using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using Development.Core.Common.ApiDtos;
using Development.Core.Core.Interface;
using Development.Core.Interface;
using Development.Core.Managers.Proxy;
using Development.Dal.Common.Model;
using Development.Dal.Error.Model;
using Newtonsoft.Json;
using NHibernate.Hql.Classic;
using OfficeOpenXml;

namespace Development.Core.Core.Managers {
    internal class CommonManager : IManager {

        /// <summary>
        /// The instance
        /// </summary>
        private static CommonManager instance = new CommonManager();

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        internal static CommonManager Instance => instance;

        /// <summary>
        /// Initializes the specified Development manager.
        /// </summary>
        /// <param name="developmentManager">The Development manager.</param>
        void IManager.Initialize(IDevelopmentManager developmentManager) {
            // Cache and initialize things here...
        }

        /// <summary>
        /// Commit all caches since the transaction has been commited.
        /// </summary>
        void IManager.CommitCaches() {
        }

        /// <summary>
        /// Rollback all caches since the transaction has been rollbacked.
        /// </summary>
        void IManager.RollbackCaches() {
        }

        #region SaveError
        public bool SaveError(CommonManagerProxy proxy, ErrorDao error) {
            try {
                using (ITransaction tx = proxy.DevelopmentManager.GetTransaction()) {
                    tx.PersistenceManager.UserRepository.Save(error);
                    tx.Commit();
                }
                return true;
            } catch (DBConcurrencyException ex) {
                return false;
            } catch (Exception ex) {
                return false;
            }

        }

        #endregion

        #region LogError
        public bool LogError(CommonManagerProxy proxy, Exception ex) {
            try {
                ErrorDao errDao = new ErrorDao {
                    DateCreated = DateTime.Now,
                    StackTrace = ex.StackTrace,
                    Message = ex.Message
                };
                SaveError(proxy, errDao);
                return true;
            } catch {
                return false;
            }
        }

        #endregion

        #region Availability Search

        public int SaveEricaConfiguration(CommonManagerProxy proxy, EricaDto dto)
        {
            try
            {
                EricaNomineeDao dao = new EricaNomineeDao
                {
                    AwardDate = dto.AwardDate,
                    Office = dto.Office,
                    Quarter = dto.Quarter,
                    SourceFileName = dto.SourceFileName,
                    Subject = dto.Subject
                };
                using (ITransaction tx = proxy.DevelopmentManager.GetTransaction())
                {
                    tx.PersistenceManager.UserRepository.Save(dao);
                    tx.Commit();
                }
                return dao.Id;
            }
            catch (Exception ex)
            {
                LogError(proxy, ex);
                return 0;
            }
        }

        #endregion

        #region

        public bool ProcessEricaExcel(CommonManagerProxy proxy, SearchRequest request, bool firstRowHasFieldNames) {

            string excelFileName = @"D:\Company-Projects\BulkEmailer\Development\Presentation\Copy-of-2017-Q2-GALE-BLR-Erica-Awards-(Responses).xlsx";

            var infile = new FileInfo(excelFileName);
            using (var exp = new ExcelPackage(infile)) {
                if (exp.Workbook.Worksheets.Count > 0) {
                    var ws = exp.Workbook.Worksheets.First();
                    var start = ws.Dimension.Start;
                    var end = ws.Dimension.End;

                    var fieldNames = new Dictionary<int, string>();
                    var firstRow = start.Row;
                    if (firstRowHasFieldNames) {
                        for (var x = start.Column; x <= end.Column; x++) {
                            fieldNames.Add(x, GetCellStringValue(ws, x, start.Row));
                        }
                        firstRow++;
                    } else {
                        for (var x = start.Column; x <= end.Column; x++) {
                            fieldNames.Add(x, $"Column_{x}");
                        }
                    }
                    var nominationList = TransformtoEricaNomineeList(ws, start, end, fieldNames, firstRow);

                    var groupedNominationList = nominationList
                        .OrderBy(nm => nm.NomineeEmail)
                        .ThenBy(nm => nm.NominatorEmail)
                        .GroupBy(u => u.NomineeEmail)
                        .Select(grp => grp.ToList())
                        .ToList();

                    foreach (var nominee in groupedNominationList) {
                        var message = string.Join($"{Environment.NewLine}",
                            nominee.OrderBy(nm => nm.NominatorEmail).Select(
                                nm =>
                                    $"{Environment.NewLine}From {nm.NominatorEmail}: {Environment.NewLine} {Environment.NewLine}{nm.Message}{Environment.NewLine}")
                            );
                    }

                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Gets the excel cell value as a string
        /// </summary>
        /// <param name="wks">The WKS.</param>
        /// <param name="row">The row.</param>
        /// <param name="col">The col.</param>
        /// <returns></returns>
        private static string GetCellStringValue(ExcelWorksheet wks, int row, int col) {
            var cVal = wks.Cells[row, col].Value;
            return cVal?.ToString();
        }

        private List<EricaNominee> TransformtoEricaNomineeList(ExcelWorksheet ws,
            ExcelCellAddress start, ExcelCellAddress end, Dictionary<int, string> fieldNames, int firstRow) {
            try {
                var ericaNomineeList = new List<EricaNominee>();
                for (var row = firstRow; row <= end.Row; row++) {
                    ericaNomineeList.Add(new EricaNominee {
                        //TODO: map the properties based on config columnn names and column number
                        NominatorEmail = GetCellStringValue(ws, row, 2),
                        NomineeName = GetCellStringValue(ws, row, 3),
                        NomineeEmail = GetCellStringValue(ws, row, 4),
                        AwardName = GetCellStringValue(ws, row, 5),
                        Message = GetCellStringValue(ws, row, 6)
                    });
                }
                return ericaNomineeList;
            } catch (Exception ex) {
                return null;
            }
        }

        #endregion



    }
}





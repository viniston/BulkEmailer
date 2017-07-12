using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Development.Core.Common.ApiDtos;
using Development.Core.Core.Interface;
using Development.Core.Core.Managers.Proxy;
using Development.Core.Interface;
using Development.Core.Utility;
using Development.Dal.Common.Model;
using Development.Dal.Error.Model;
using EASendMail;
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

        #region Save Erica nomination settings

        public List<EricaNomineeDao> SaveEricaConfiguration(CommonManagerProxy proxy, EricaDto dto) {
            try {
                List<EricaNomineeDao> lst;
                EricaNomineeDao dao = new EricaNomineeDao {
                    AwardDate = dto.AwardDate,
                    Office = dto.Office,
                    Quarter = dto.Quarter,
                    SourceFileName = dto.SourceFileName,
                    Subject = dto.Subject,
                    DateCreated = DateTime.Now
                };
                using (ITransaction tx = proxy.DevelopmentManager.GetTransaction()) {
                    tx.PersistenceManager.UserRepository.Save(dao);
                    tx.Commit();
                    ProcessEricaExcel(proxy, dao);
                    lst = tx.PersistenceManager.UserRepository.GetAll<EricaNomineeDao>().ToList();
                }
                return lst;
            } catch (Exception ex) {
                LogError(proxy, ex);
                return null;
            }
        }

        #endregion

        #region

        public bool ProcessEricaExcel(CommonManagerProxy proxy, EricaNomineeDao request) {


            var baseBath = ConfigurationManager.AppSettings["AppContent"];
            var excelFileName = baseBath + @"EricaTemplates\" + request.SourceFileName;

            // string excelFileName = @"D:\Company-Projects\BulkEmailer\Development\Presentation\Copy-of-2017-Q2-GALE-BLR-Erica-Awards-(Responses).xlsx";

            var infile = new FileInfo(excelFileName);
            IList<EricaNomineeListDao> nomineeList = new List<EricaNomineeListDao>();
            using (var exp = new ExcelPackage(infile)) {
                if (exp.Workbook.Worksheets.Count > 0) {
                    var ws = exp.Workbook.Worksheets.First();
                    var start = ws.Dimension.Start;
                    var end = ws.Dimension.End;

                    var fieldNames = new Dictionary<int, string>();
                    var firstRow = start.Row;
                    for (var x = start.Column; x <= end.Column; x++) {
                        fieldNames.Add(x, GetCellStringValue(ws, x, start.Row));
                    }
                    firstRow++;
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
                        var nomineeDto = nominee.FirstOrDefault();
                        if (!string.IsNullOrEmpty(nomineeDto?.NomineeEmail))
                            nomineeList.Add(new EricaNomineeListDao {
                                AwardName = string.Join(", ", nominee.Select(a => a.AwardName).ToArray()),
                                EricaId = request.Id,
                                Message = EmailMessagehelper.GetEmailBodyMessage(nominee),
                                NominatorEmailList =
                                    string.Join(", ", nominee.Select(a => a.NominatorEmail).ToArray()),
                                NomineeEmail = nomineeDto.NomineeEmail,
                                NomineeName = nomineeDto.NomineeName,
                                Status = "In progress"
                            });
                    }
                    using (ITransaction tx = proxy.DevelopmentManager.GetTransaction()) {

                        if (nomineeList.Any()) {
                            tx.PersistenceManager.UserRepository.Save(nomineeList);
                            tx.Commit();
                        }

                        return true;
                    }
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


        public List<EricaNomineeDao> GetEricaTemplates(CommonManagerProxy proxy) {
            try {
                List<EricaNomineeDao> lst;
                using (ITransaction tx = proxy.DevelopmentManager.GetTransaction()) {
                    lst = tx.PersistenceManager.UserRepository.GetAll<EricaNomineeDao>().ToList();
                }
                return lst;
            } catch (Exception ex) {
                LogError(proxy, ex);
                return null;
            }
        }

        public List<EricaNomineeListDao> GetEricaNomineeList(CommonManagerProxy proxy, int ericaId) {
            try {
                List<EricaNomineeListDao> lst;
                using (ITransaction tx = proxy.DevelopmentManager.GetTransaction()) {
                    lst =
                        tx.PersistenceManager.UserRepository.GetAll<EricaNomineeListDao>()
                            .Where(e => e.EricaId == ericaId)
                            .ToList();
                }
                return lst;
            } catch (Exception ex) {
                LogError(proxy, ex);
                return null;
            }
        }

        public EricaNomineeListDao GetEricaNominatorMessage(CommonManagerProxy proxy, int nominationId) {
            try {
                EricaNomineeListDao dao;
                using (ITransaction tx = proxy.DevelopmentManager.GetTransaction()) {
                    dao =
                        tx.PersistenceManager.UserRepository
                            .GetAll<EricaNomineeListDao>().FirstOrDefault(e => e.Id == nominationId);
                }
                return dao;
            } catch (Exception ex) {
                LogError(proxy, ex);
                return null;
            }
        }


        public bool SendEmail(CommonManagerProxy proxy, EmailDto dto) {

            try {

                using (ITransaction tx = proxy.DevelopmentManager.GetTransaction()) {
                    var nominationDao =
                        tx.PersistenceManager.UserRepository
                            .GetAll<EricaNomineeListDao>().FirstOrDefault(e => e.Id == dto.NominationId);

                    var ericaDao =
                        tx.PersistenceManager.UserRepository
                            .GetAll<EricaNomineeDao>()
                            .FirstOrDefault(e => nominationDao != null && e.Id == nominationDao.EricaId);
                    if (nominationDao != null && ericaDao != null) {

                        SmtpMail oMail = new SmtpMail("TryIt");
                        SmtpClient oSmtp = new SmtpClient();

                        // Your Offic 365 email address
                        oMail.From = ConfigurationManager.AppSettings["ericaMail"];

                        // Set recipient email address
                        oMail.To = nominationDao.NomineeEmail;

                        // Set email subject
                        oMail.Subject = ericaDao.Subject;

                        //Local File
                        var htmlCode = EmailMessagehelper.GetHtmlMailBody(dto.MailBody);

                        oMail.HtmlBody = htmlCode;

                        // Your Office 365 SMTP server address,
                        // You should get it from outlook web access.
                        SmtpServer oServer = new SmtpServer("smtp.office365.com") {
                            User = ConfigurationManager.AppSettings["senderMail"],
                            Password = ConfigurationManager.AppSettings["senderPwd"],
                            Port = 587,
                            ConnectType = SmtpConnectType.ConnectSSLAuto
                        };
                        try {
                            oSmtp.SendMail(oServer, oMail);

                            nominationDao.Status = "Mail Sent";
                            tx.PersistenceManager.UserRepository.Save(nominationDao);


                            return true;
                        } catch (Exception ep) {
                            return false;
                        }
                    }
                    tx.Commit();
                    return false;
                }
            } catch (Exception ex) {
                return false;
            }
        }


    }
}





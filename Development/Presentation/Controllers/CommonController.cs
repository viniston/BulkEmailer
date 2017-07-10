using Development.Core;
using Development.Core.Interface;
using Development.Web.Controllers;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web;
using System.Web.Http;
using Development.Core.Common.ApiDtos;
using Development.Core.Core.Interface;
using Development.Web.Helper;
using Development.Web.Models;

namespace Development.Web.Controllers {

    [RoutePrefix("api/Common")]
    public class CommonController : ApiController {
        private ServiceResponse _result;

        [HttpPost]
        [Route("SearchAvailabilty")]
        public ServiceResponse SearchAvailabilty(SearchRequestDto dto) {
            _result = new ServiceResponse();
            try {
                if (ModelState.IsValid) {
                    Guid systemSession = DevelopmentManagerFactory.GetSystemSession();
                    IDevelopmentManager developmentManager = DevelopmentManagerFactory.GetDevelopmentManager(systemSession);
                    _result.StatusCode = (int)HttpStatusCode.OK;
                    SearchRequest request = new SearchRequest {
                        LoginDetails = new SearchRequestLoginDetails {
                            Login = ConfigurationManager.AppSettings["APIUser"],
                            Password = ConfigurationManager.AppSettings["APIPassword"]
                        },
                        SearchDetails = new SearchRequestSearchDetails {
                            ArrivalDate = Convert.ToDateTime(dto.ArrivalDate),
                            Duration = dto.Duration,
                            MealBasisID = 0,
                            MinStarRating = dto.MinStarRating,
                            //TODO: comment this if we go for property search
                            //RegionID = 72, // TODO: need to get all their region list 
                            PropertyReferenceIDs = new PropertyReferenceIDs {
                                PropertyReferenceID = new int[] { 68851 } //Leopold Hotel Brussels
                            },
                            RoomRequests = new SearchRequestSearchDetailsRoomRequests {
                                RoomRequest = new SearchRequestSearchDetailsRoomRequestsRoomRequest {
                                    Adults = dto.Adults,
                                    Children = dto.Children,
                                    Infants = dto.Infants
                                }
                            }
                        }
                    };
                    _result.Response = developmentManager.CommonManager.AvailabilitySearch(request);
                } else {
                    _result.StatusCode = (int)HttpStatusCode.BadRequest;
                    _result.Response = null;
                }
            } catch {
                _result.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
                _result.Response = null;
            }
            return _result;
        }

        [HttpPost]
        [Route("UploadEricaTemplateWithData")]
        public ServiceResponse UploadEricaTemplateWithData() {
            _result = new ServiceResponse();
            try {
                // get variables first
                var nvc = HttpContext.Current.Request.Form;
                var model = new ErricaModel();
                // iterate through and map to strongly typed model
                foreach (var kvp in nvc.AllKeys) {
                    var pi = model.GetType().GetProperty(kvp, BindingFlags.Public | BindingFlags.Instance);
                    int i;
                    DateTime j;
                    if (int.TryParse(nvc[kvp], out i)) {
                        pi?.SetValue(model, i, null);
                    } else if (DateTime.TryParse(nvc[kvp], out j)) {
                        pi?.SetValue(model, j, null);
                    } else {
                        pi?.SetValue(model, nvc[kvp], null);
                    }

                }
                var fileName = UIhelper.GenerateUniqueId() +
                                  "_" + model.SourceFile.FileName;
                model.SourceFile = HttpContext.Current.Request.Files["SourceFile"];

                if (model.SourceFile != null) {
                    var baseBath = ConfigurationManager.AppSettings["AppContent"];
                    var filePath =
                        HttpContext.Current.Server.MapPath(baseBath + @"EricaTemplates\" + fileName);
                    model.SourceFile.SaveAs(filePath);
                }


                _result = new ServiceResponse { StatusCode = (int)HttpStatusCode.OK, Response = 1 };
            } catch (Exception ex) {
                _result.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
                _result.Response = null;
            }
            return _result;
        }
    }
}
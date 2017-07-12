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
        [Route("UploadEricaTemplateWithData")]
        public ServiceResponse UploadEricaTemplateWithData() {
            _result = new ServiceResponse();
            try {
                // get variables first
                var nvc = HttpContext.Current.Request.Form;
                var request = new EricaDto();
                // iterate through and map to strongly typed model
                foreach (var kvp in nvc.AllKeys) {
                    var pi = request.GetType().GetProperty(kvp, BindingFlags.Public | BindingFlags.Instance);
                    int i;
                    if (int.TryParse(nvc[kvp], out i)) {
                        pi?.SetValue(request, i, null);
                    } else {
                        pi?.SetValue(request, nvc[kvp], null);
                    }

                }
                request.SourceFile = HttpContext.Current.Request.Files["SourceFile"];
                if (request.SourceFile != null) {
                    var fileName = UIhelper.GenerateUniqueId() +
                                   "_" + request.SourceFile.FileName;

                    if (request.SourceFile != null) {
                        var baseBath = ConfigurationManager.AppSettings["AppContent"];
                        var filePath = baseBath + @"EricaTemplates\" + fileName;
                        request.SourceFile.SaveAs(filePath);
                    }

                    request.SourceFileName = fileName;
                }

                Guid systemSession = DevelopmentManagerFactory.GetSystemSession();
                IDevelopmentManager developmentManager = DevelopmentManagerFactory.GetDevelopmentManager(systemSession);
                _result.StatusCode = (int)HttpStatusCode.OK;
                _result.Response = developmentManager.CommonManager.SaveEricaConfiguration(request);
            } catch (Exception ex) {
                _result.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
                _result.Response = null;
            }
            return _result;
        }

        [HttpGet]
        [Route("GetHistory")]
        public ServiceResponse GetHistory() {
            _result = new ServiceResponse();
            try {

                Guid systemSession = DevelopmentManagerFactory.GetSystemSession();
                IDevelopmentManager developmentManager = DevelopmentManagerFactory.GetDevelopmentManager(systemSession);
                _result.StatusCode = (int)HttpStatusCode.OK;
                _result.Response = developmentManager.CommonManager.GetEricaTemplates();
            } catch {
                _result.StatusCode = (int)HttpStatusCode.InternalServerError;
                _result.Response = null;
            }
            return _result;
        }

        [HttpGet]
        [Route("GetEricaNomineeList/{ericaID}")]
        public ServiceResponse GetEricaNomineeList(int ericaId) {
            _result = new ServiceResponse();
            try {

                Guid systemSession = DevelopmentManagerFactory.GetSystemSession();
                IDevelopmentManager developmentManager = DevelopmentManagerFactory.GetDevelopmentManager(systemSession);
                _result.StatusCode = (int)HttpStatusCode.OK;
                _result.Response = developmentManager.CommonManager.GetEricaNomineeList(ericaId);
            } catch {
                _result.StatusCode = (int)HttpStatusCode.InternalServerError;
                _result.Response = null;
            }
            return _result;
        }

        [HttpGet]
        [Route("GetEricaNominatorMessage/{nominationID}")]
        public ServiceResponse GetEricaNominatorMessage(int nominationId) {
            _result = new ServiceResponse();
            try {

                Guid systemSession = DevelopmentManagerFactory.GetSystemSession();
                IDevelopmentManager developmentManager = DevelopmentManagerFactory.GetDevelopmentManager(systemSession);
                _result.StatusCode = (int)HttpStatusCode.OK;
                _result.Response = developmentManager.CommonManager.GetEricaNominatorMessage(nominationId);
            } catch {
                _result.StatusCode = (int)HttpStatusCode.InternalServerError;
                _result.Response = null;
            }
            return _result;
        }

        [HttpPost]
        [Route("SendMail")]
        public ServiceResponse SendMail(EmailModel model)
        {
            _result = new ServiceResponse();
            try
            {

                Guid systemSession = DevelopmentManagerFactory.GetSystemSession();
                IDevelopmentManager developmentManager = DevelopmentManagerFactory.GetDevelopmentManager(systemSession);
                _result.StatusCode = (int) HttpStatusCode.OK;
                var mailDto = new EmailDto
                {
                    NominationId = model.NominationId,
                    MailBody = model.MailBody
                };
                _result.Response = developmentManager.CommonManager.SendEmail(mailDto);
            }
            catch
            {
                _result.StatusCode = (int) HttpStatusCode.InternalServerError;
                _result.Response = null;
            }
            return _result;
        }
    }
}
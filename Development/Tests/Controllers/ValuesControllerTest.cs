using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Development.Core;
using Development.Core.Common.ApiDtos;
using Development.Core.Core.Interface;
using Development.Core.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;



namespace Development.Tests.Controllers {
    [TestClass]
    public class ValuesControllerTest {

        [TestInitialize]
        void InitializeAllDevelopmentInstances() {
            //intialize all Development tenants and NHibernate mappings and all
            DevelopmentManagerFactory.InitializeSystem();
        }

        [TestMethod]
        public void TestExcelProcessing() {
            InitializeAllDevelopmentInstances();
            var systemSession = DevelopmentManagerFactory.GetSystemSession();
            var developmentManager = DevelopmentManagerFactory.GetDevelopmentManager(systemSession);
            var response = developmentManager.CommonManager.ProcessEricaExcel(new SearchRequest(), true);
        }

    }
}

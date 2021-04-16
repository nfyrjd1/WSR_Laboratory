using Microsoft.VisualStudio.TestTools.UnitTesting;
using WSR_Laboratory_Analyzer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSR_Laboratory_Analyzer.Tests
{
    [TestClass()]
    public class AnalyzerTests
    {
        [TestMethod()]
        public void Check_AnalyzerName()
        {
            Analyzer Analyzer = new Analyzer();
            string AnalyzerName = "Ledetect1";
            string Expected = $"Ошибка 400. Analyzer with name 'Ledetect1' not found.";

            string Response = Analyzer.Get(AnalyzerName);

            Assert.AreEqual(Expected, Response);
        }

        [TestMethod()]
        public void Check_AnalyzerNotWorking()
        {
            Analyzer Analyzer = new Analyzer();
            string AnalyzerName = "Ledetect";
            string Expected = "Ошибка 400. Analyzer is not working.";

            string Response = Analyzer.Get(AnalyzerName);

            Assert.AreEqual(Expected, Response);
        }

        [TestMethod()]
        public void Check_AnalyzerServices()
        {
            Analyzer Analyzer = new Analyzer();
            string AnalyzerName = "Ledetect";
            string[] Services = new string[] { "Амилаза", "Спид", "TSH" };
            string Expected = "Ошибка 400. Analyzer can not do this order. May be order contains services which analyzer does not support.";

            string Response = Analyzer.Post(AnalyzerName, Services);

            Assert.AreEqual(Expected, Response);
        }

        [TestMethod()]
        public void Check_AnalyzerBusy()
        {
            Analyzer Analyzer = new Analyzer();
            string AnalyzerName = "Ledetect";
            string[] Services = new string[] { "Амилаза", "Креатинин" };
            string Expected = "Ошибка 400. Analyzer is busy.";

            Analyzer.Post(AnalyzerName, Services);
            string Response = Analyzer.Post(AnalyzerName, Services);

            Assert.AreEqual(Expected, Response);
        }
    }
}
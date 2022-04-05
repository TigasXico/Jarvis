
using System;
using System.Linq;
using Jarvis.Data.Contract;
using Jarvis.Data.DataAccess.Extractors;
using Jarvis.Data.DataAccess.Scraping;
using Jarvis.Data.DataModels;
using JARVIS.TestFramework.Controllers.ModelControllers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JARVIS.TestFramework.Data.DataAcess.Extractors
{
    [TestClass]
    public class BasicInfoDataExtractorTests
    {
        private IWebScraper<FiscalEntityDataModel> webScraper;
        private IWebScraper<FiscalEntityDataModel> WebScraper
        {
            get
            {
                if ( webScraper == null )
                {
                    webScraper = new FinancesWebScraperFactory().GetScraper();
                }

                return webScraper;
            }
        }

        [TestMethod]
        public void TestClientBasicInfoExtraction()
        {
            var testClientDataModel = FiscalEntityControllerTests.GetTestClientDataModel();

            WebScraper.LoginEntity( testClientDataModel );

            BasicInfoDataExtractor.GetData( testClientDataModel , WebScraper );

            Assert.IsFalse( string.IsNullOrWhiteSpace( testClientDataModel.FiscalNumber ) );

            Assert.IsFalse( string.IsNullOrWhiteSpace( testClientDataModel.FiscalPassword ) );

            Assert.IsFalse( string.IsNullOrWhiteSpace( testClientDataModel.Name ) );

            Assert.IsFalse( string.IsNullOrWhiteSpace( testClientDataModel.BirthDate ) );

            Assert.IsFalse( string.IsNullOrWhiteSpace( testClientDataModel.Gender ) );

            Assert.IsFalse( string.IsNullOrWhiteSpace( testClientDataModel.Nationality ) );

            Assert.IsFalse( string.IsNullOrWhiteSpace( testClientDataModel.FiscalAddress ) );

            Assert.IsFalse( string.IsNullOrWhiteSpace( testClientDataModel.FiscalAddressAdditionalInfo ) );

            Assert.IsFalse( string.IsNullOrWhiteSpace( testClientDataModel.FiscalAddressZipCode ) );

            Assert.IsFalse( string.IsNullOrWhiteSpace( testClientDataModel.FinancialServicesRepartition ) );
        }

        [TestMethod]
        public void TestCompanyBasicInfoExtraction()
        {
            var testCompanyDataModel = FiscalEntityControllerTests.GetTestCompanyDataModel();

            using ( WebScraper )
            {
                WebScraper.LoginEntity( testCompanyDataModel );

                BasicInfoDataExtractor.GetData( testCompanyDataModel , WebScraper );
            }

            Assert.IsFalse( string.IsNullOrWhiteSpace( testCompanyDataModel.FiscalNumber ) );

            Assert.IsFalse( string.IsNullOrWhiteSpace( testCompanyDataModel.FiscalPassword ) );

            Assert.IsFalse( string.IsNullOrWhiteSpace( testCompanyDataModel.Name ) );

            Assert.IsFalse( string.IsNullOrWhiteSpace( testCompanyDataModel.FiscalAddress ) );

            Assert.IsFalse( string.IsNullOrWhiteSpace( testCompanyDataModel.FiscalAddressAdditionalInfo ) );

            Assert.IsFalse( string.IsNullOrWhiteSpace( testCompanyDataModel.FiscalAddressZipCode ) );

            Assert.IsFalse( string.IsNullOrWhiteSpace( testCompanyDataModel.FinancialServicesRepartition ) );

        }

        [TestMethod]
        public void TestContactsInfoExtraction()
        {
            var testFiscalEntity = FiscalEntityControllerTests.GetTestFiscalEntityDataModel(FiscalEntityTypes.Client);

            using ( WebScraper )
            {
                WebScraper.LoginEntity( testFiscalEntity );

                ContactInfoExtractor.GetData( testFiscalEntity , WebScraper );
            }

            Assert.IsNotNull( testFiscalEntity.Contacts );

            Assert.AreEqual( 2 , testFiscalEntity.Contacts.Count );

            var currentContactModel = testFiscalEntity.Contacts[0];

            Assert.AreEqual( ContactType.Email , currentContactModel.ContactType );

            Assert.AreEqual( "fmcfisco@gmail.com" , currentContactModel.ContactValue );

            currentContactModel = testFiscalEntity.Contacts[1];

            Assert.AreEqual( ContactType.PhoneNumber, currentContactModel.ContactType );

            Assert.AreEqual( "229384290" , currentContactModel.ContactValue );

        }

        [TestMethod]
        public void TestVehiecleInfoExtraction()
        {
            var testFiscalEntity = FiscalEntityControllerTests.GetTestFiscalEntityDataModel( FiscalEntityTypes.WithVehiecles);

            using ( WebScraper )
            {
                WebScraper.LoginEntity( testFiscalEntity );

                VehiecleInfoExtractor.GetData( testFiscalEntity , WebScraper );
            }

            Assert.IsNotNull( testFiscalEntity.Vehiecles );

            Assert.AreEqual( 1 , testFiscalEntity.Vehiecles.Count );

            var vehiecle = testFiscalEntity.Vehiecles.Single();

            Assert.AreEqual( "Hyundai" , vehiecle.Brand , true );

            Assert.AreEqual( "AB-24-MC" , vehiecle.LicensePlate , true );

            Assert.AreEqual( new DateTime( 2020 , 06 , 30 ) , vehiecle.DateOfLicensePlate );

            Assert.AreEqual( "junho de 2020" , vehiecle.DateOfLicensePlateString , true );

            Assert.AreEqual( "PDE" , vehiecle.Model , true );

            Assert.AreEqual( "Proprietário" , vehiecle.RoleOfClient , true );
        }

        [TestMethod]
        public void TestRealEstateInfoExtraction()
        {
            var testFiscalEntity = FiscalEntityControllerTests.GetTestFiscalEntityDataModel(FiscalEntityTypes.WithRealEstates);

            using ( WebScraper )
            {
                WebScraper.LoginEntity( testFiscalEntity );

                RealEstateInfoExtractor.GetData( testFiscalEntity , WebScraper );
            }

            Assert.IsNotNull( testFiscalEntity.RealEstates);

            Assert.AreEqual( 1 , testFiscalEntity.RealEstates.Count );

            var realEstateModel = testFiscalEntity.RealEstates.Single();

            Assert.AreEqual( "U-3854-R" , realEstateModel.FullArticle , true );

            Assert.AreEqual( "46812.36" , realEstateModel.CurrentValue , true );

            Assert.AreEqual( "34691.34" , realEstateModel.InitialValue , true );

            Assert.AreEqual( "131218 - UNIÃO DAS FREGUESIAS DE LORDELO DO OURO E MASSARELOS" , realEstateModel.Location , true );

            Assert.AreEqual( 1993 , realEstateModel.MatrixYear );
            
            Assert.AreEqual( "U" , realEstateModel.Type , true );
        }
    }
}


using Jarvis.Controllers.ModelControllers;
using Jarvis.Data.DataModels;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Collections.Generic;
using System.Linq;

namespace JARVIS.TestFramework.Controllers.ModelControllers
{
    [TestClass]
    public class FiscalEntityControllerTests
    {
        #region Constants

        public static readonly List<string> validClientsFiscalNumbers = new List<string>()
        {
            "178599131" ,
            "250418681" 
        };

        public static readonly List<string> invalidClientsFiscalNumbers = new List<string>()
        {
            "178599132",
            "250418682"
        };

        public static readonly List<string> validCompaniesFiscalNumbers = new List<string>()
        {
            "505901447"
        };

        public static readonly List<string> invalidCompaniesFiscalNumbers = new List<string>()
        {
            "505901446"
        };

        public static readonly Dictionary<string , (FiscalEnityType, bool)> clientFiscalNumberToTypeAndValidityMapping = 
            validClientsFiscalNumbers.ToDictionary( i => i , i => (FiscalEnityType.Client, true) )
            .Concat( invalidClientsFiscalNumbers.ToDictionary( i => i , i => (FiscalEnityType.Client, false) ) )
            .ToDictionary( k => k.Key , v => v.Value );

        public static readonly Dictionary<string , (FiscalEnityType, bool)> companyFiscalNumberToTypeAndValidityMapping =
            validCompaniesFiscalNumbers.ToDictionary( i => i , i => (FiscalEnityType.Company, true) )
            .Concat( invalidCompaniesFiscalNumbers.ToDictionary(i => i , i => (FiscalEnityType.Company, false) ) )
            .ToDictionary( k => k.Key , v => v.Value );

        public static readonly Dictionary<string , (FiscalEnityType, bool)> fiscalNumberToTypeAndValidityMapping =
            clientFiscalNumberToTypeAndValidityMapping
            .Concat( companyFiscalNumberToTypeAndValidityMapping )
            .ToDictionary( k => k.Key , v => v.Value );

        #endregion

        [TestMethod]
        public void FiscalNumberValidation()
        {
            foreach ( var entry in fiscalNumberToTypeAndValidityMapping )
            {
                Assert.AreEqual( entry.Value.Item2 , FiscalEntityController.IsFiscalNumberValid( entry.Key ) );
            }
        }

        [TestMethod]
        public void FiscalNumberCategortization()
        {
            foreach ( var entry in clientFiscalNumberToTypeAndValidityMapping )
            {
                Assert.IsTrue( FiscalEntityController.IsFiscalNumberOfType( entry.Key , entry.Value.Item1 ) );
            }

            foreach ( var entry in companyFiscalNumberToTypeAndValidityMapping )
            {
                Assert.IsTrue( FiscalEntityController.IsFiscalNumberOfType( entry.Key , entry.Value.Item1 ) );
            }
        }

        [TestMethod]
        public void FiscalInfoResolvedToClient()
        {
            GetTestClientDataModel();

            var errorType = FiscalEntityController.ResolveFiscalInfoToEntity( "178599131" , "3726CISCO" , out var expectedToBeclient );

            Assert.AreEqual( errorType , FiscalEntityGenerationResult.NoError );

            Assert.IsNotNull( expectedToBeclient );

            Assert.IsInstanceOfType( expectedToBeclient , typeof( ClientDataModel ) );

        }

        [TestMethod]
        public void FiscalInfoResolvedToCompany()
        {
            GetTestCompanyDataModel();
        }

        public static FiscalEntityDataModel GetTestFiscalEntityDataModel( FiscalEntityTypes typeOfEntity )
        {
            FiscalEntityDataModel fiscalEntityToReturn;
            FiscalEntityGenerationResult errorType;
            switch ( typeOfEntity )
            {
                case FiscalEntityTypes.Client:
                    errorType = FiscalEntityController.ResolveFiscalInfoToEntity( "250418681" , "TRF250418" , out fiscalEntityToReturn );
                    break;
                case FiscalEntityTypes.Company:
                    errorType = FiscalEntityController.ResolveFiscalInfoToEntity( "505901447" , "JF505901" , out fiscalEntityToReturn );
                    break;
                case FiscalEntityTypes.WithVehiecles:
                    errorType = FiscalEntityController.ResolveFiscalInfoToEntity( "250418681" , "TRF250418" , out fiscalEntityToReturn );
                    break;
                case FiscalEntityTypes.WithRealEstates:
                    errorType = FiscalEntityController.ResolveFiscalInfoToEntity( "178599131" , "3726CISCO" , out fiscalEntityToReturn );
                    break;
                default:
                    errorType = FiscalEntityGenerationResult.Default;
                    fiscalEntityToReturn = null;
                    break;
            }

            Assert.AreEqual( errorType , FiscalEntityGenerationResult.NoError );

            Assert.IsNotNull( fiscalEntityToReturn );

            return fiscalEntityToReturn;
        }

        public static ClientDataModel GetTestClientDataModel()
        {
            var errorType = FiscalEntityController.ResolveFiscalInfoToEntity( "250418681" , "TRF250418" , out var expectedToBeclient );

            Assert.AreEqual( errorType , FiscalEntityGenerationResult.NoError );

            Assert.IsNotNull( expectedToBeclient );

            Assert.IsInstanceOfType( expectedToBeclient , typeof( ClientDataModel ) );

            return expectedToBeclient as ClientDataModel;
        }

        public static CompanyDataModel GetTestCompanyDataModel()
        {
            var errorType = FiscalEntityController.ResolveFiscalInfoToEntity( "505901447" , "JF505901" , out var expectedToBecompany );

            Assert.AreEqual( errorType , FiscalEntityGenerationResult.NoError );

            Assert.IsNotNull( expectedToBecompany );

            Assert.IsInstanceOfType( expectedToBecompany , typeof( CompanyDataModel ) );

            return expectedToBecompany as CompanyDataModel;
        }
    }

    public enum FiscalEntityTypes
    {
        Client,
        Company,
        WithVehiecles,
        WithRealEstates
    }
}

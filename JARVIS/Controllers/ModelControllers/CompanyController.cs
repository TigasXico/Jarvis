using System;
using System.ComponentModel;
using Jarvis.Data.Contract.Repositories;
using Jarvis.Data.DataModels;
using Jarvis.Services;

namespace Jarvis.Controllers.ModelControllers
{
    public class CompanyController : FiscalEntityController
    {
        public CompanyController( CompanyDataModel company , BackgroundWorker worker , IUnitOfWork unitOfWork = null ) : base(  worker , unitOfWork )
        {
            var obtainedCompanyFromDatabase = UnitOfWork.Companies.GetByFiscalNumber( company.FiscalNumber );

            if ( obtainedCompanyFromDatabase != default( CompanyDataModel ) )
            {
                Model = obtainedCompanyFromDatabase;
            }
            else
            {
                UnitOfWork.Companies.Add( company );
                Model = company;
                Model.IsNew = true;
            }

            PrepareCollectionsForUpdate();
        }

        public override bool DeleteEntity()
        {
            try
            {
                UnitOfWork.Companies.RemoveEntity( Model as CompanyDataModel);
                return true;
            }
            catch (Exception ex)
            {
                WindowService.ShowException( ex );
                return false;
            }
        }
    }
}

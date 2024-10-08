﻿using System;

using Jarvis.Data.Contract.Repositories;

namespace Jarvis.DataAcess.Contract
{
    public interface IUnitOfWork : IDisposable
    {
        IClientRepository Clients
        {
            get;
        }

        ICompanyRepository Companies
        {
            get;
        }

        IContactsRepository Contacts
        {
            get;
        }

        IVehiecleRepository Vehiecles
        {
            get;
        }

        IRealEstateRepository RealEstates
        {
            get;
        }

        ICustomerGroupRepository CustomerGroups
        {
            get;
        }

        IAggregatesRepository Aggregates
        {
            get;
        }

        ITransactionRepository Transactions
        {
            get;
        }

        IImiChargeNotesRepository ImiChargeNotes
        {
            get;
        }

        int Complete();
    }
}

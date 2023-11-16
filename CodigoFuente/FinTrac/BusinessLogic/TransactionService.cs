﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Domain.Exceptions;
using Domain.DataTypes;

namespace BusinessLogic
{
    public class TransactionService
    {
        private readonly FintracContext _database;

        public TransactionService(FintracContext database)
        {
            _database = database;
        }

        public void Add(Account account, Transactions transaction)
        {
            if (account.TransactionList.Contains(transaction))
            {
                throw new TransactionAlreadyExistsException();
            }
            var user = _database.Users.FirstOrDefault(x => x.WorkspaceList.Contains(account.WorkSpace));
            if (user == null)
            {
                throw new ArgumentNullException();
            }
            var targetWorkspace = user.WorkspaceList.FirstOrDefault(x => x.ID == account.WorkSpace.ID);
            if (targetWorkspace == null)
            {
                throw new ArgumentNullException();
            }
            var exchange = targetWorkspace.ExchangeList.Find(x => x.Date <= transaction.CreationDate && x.Currency == transaction.Currency);
            if (exchange == null && transaction.Currency != CurrencyType.UYU)
            {
                throw new ExchangeNotFoundException();
            }
            try
            {
                account.TransactionList.Add(transaction);
            }
            catch (Exception exception)
            {
                throw exception;
            }
            _database.SaveChanges();
        }

        public void Duplicate(Account account, Transactions transaction)
        {
            Transactions duplicatedTransaction = new Transactions
            {
                Title = transaction.Title,
                Amount = transaction.Amount,
                CreationDate = DateTime.Today,
                Currency = transaction.Currency,
                Category = transaction.Category,
                Account = transaction.Account
            };
            account.TransactionList.Add(duplicatedTransaction);
            _database.SaveChanges();
        }

        public Transactions Get(Account account, int transactionID)
        {
            User user = _database.Users.FirstOrDefault(x => x.WorkspaceList.Contains(account.WorkSpace));
            if (user == null)
            {
                return null;
            }
            Workspace workspace = user.WorkspaceList.FirstOrDefault(x => x.ID == account.WorkSpace.ID);
            if (workspace == null)
            {
                return null;
            }
            Account accountToSearch = workspace.AccountList.FirstOrDefault(x => x == account);
            if (accountToSearch == null)
            {
                return null;
            }
            Transactions transaction = accountToSearch.TransactionList.FirstOrDefault(x => x.ID == transactionID);
            if (transaction == null)
            {
                return null;
            }
            return transaction;
        }

        public void Modify(Transactions transaction, string title, double amount)
        {
            Transactions transactionToUpdate = Get(transaction.Account, transaction.ID);
            transactionToUpdate.Amount = amount;
            transactionToUpdate.Title = title;

            _database.SaveChanges();
        }
    }
}

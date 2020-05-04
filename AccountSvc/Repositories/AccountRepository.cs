﻿using AccountSvc.Models;
using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountSvc.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly string _connStr;
        private readonly string insert = "INSERT INTO account (name, email, password, created_at, last_updated, address, city, region, postal_code, country, subscribe_newsletter) values (@name, @email, @password, sysdate(), sysdate(), @address, @city, @region, @postal_code, @country, @subscribe_newsletter)";
        private readonly string updateAccount = "UPDATE account set name = @name, email = @email, last_updated = sysdate() WHERE id = @id";
        private readonly string updatePwd = "UPDATE account set password = @password WHERE id = @id";
        private readonly string queryAcctById = "SELECT * FROM account WHERE id = @id";
        private readonly string queryAcctByEmail = "SELECT * FROM account WHERE email = @email";

        public AccountRepository(string connStr)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            _connStr = connStr;
        }

        public async Task CreateAccount(Account account)
        {
            // todo :: salt + hash pwd
            // todo :: add Address VO
            using (var conn = new MySqlConnection(_connStr))
            {
                await conn.ExecuteAsync(insert, new
                {
                    name = account.Name,
                    email = account.Email,
                    password = account.Password,
                    address = account.Address,
                    city = account.City,
                    region = account.Region,
                    postal_code = account.PostalCode,
                    country = account.Country,
                    subscribe_newsletter = account.SubscribedToNewsletter
                });
            }
        }

        public async Task UpdateAccount(UpdateAccount updAccount)
        {
            using (var conn = new MySqlConnection(_connStr))
            {
                // todo :: add Address VO
                // todo :: log account history
                await conn.ExecuteAsync(updateAccount, new
                {
                    id = updAccount.Id,
                    name = updAccount.Name,
                    email = updAccount.Email
                    //address = account.Address,
                    //city = account.City,
                    //region = account.Region,
                    //postal_code = account.PostalCode,
                    //country = account.Country,
                    //subscribe_newsletter = account.SubscribedToNewsletter
                });
            }
        }

        public async Task<Account> GetAccountById(string id)
        {
            using (var conn = new MySqlConnection(_connStr))
            {
                return await conn.QuerySingleOrDefaultAsync<Account>(queryAcctById, new { id });
            }
        }

        public async Task<Account> GetAccountByEmail(string email)
        {
            using (var conn = new MySqlConnection(_connStr))
            {
                return await conn.QuerySingleOrDefaultAsync<Account>(queryAcctByEmail, new { email });
            }
        }

        public async Task UpdatePassword(UpdatePassword updPassword)
        {
            using (var conn = new MySqlConnection(_connStr))
            {
                // todo :: log account history
                await conn.QuerySingleOrDefaultAsync<Account>(updatePwd, new {
                    password = updPassword.NewPassword,
                    id = updPassword.AccountId
                });
            }
        }
    }
}
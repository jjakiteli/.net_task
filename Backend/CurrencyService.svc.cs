using Backend.Db;
using Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Backend
{
    public class CurrencyService : ICurrencyService
    {
        static CurrencyService()
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<MyDbContext>());

            using (var context = new MyDbContext())
            {
                context.Database.Initialize(false);
            }
        }

        public List<CurrencyRead> GetCurrencies()
        {
            using (var context = new MyDbContext())
            {
                return context.Currencies.Select(
                    c => new CurrencyRead
                    {
                        Id = c.Id,
                        Name = c.Name
                    }).ToList();
            }
        }

        private Currency FindCurrency(MyDbContext context, int id)
        {
            var currency = context.Currencies.Find(id);
            if (currency == null)
            {
                throw new Exception("Currency not found.");
            }
            return currency;
        }

        public CurrencyRead GetCurrency(int id)
        {
            using (var context = new MyDbContext())
            {
                var currency = FindCurrency(context, id);

                return new CurrencyRead
                {
                    Id = currency.Id,
                    Name = currency.Name
                };
            }
        }

        public void AddCurrency(CurrencyCreate newCurrency)
        {
            using (var context = new MyDbContext())
            {
                context.Currencies.Add(new Currency
                {
                    Name = newCurrency.Name
                });

                context.SaveChanges();
            }
        }

        public void UpdateCurrency(CurrencyUpdate newCurrency)
        {
            using (var context = new MyDbContext())
            {
                var currency = FindCurrency(context, newCurrency.Id);

                currency.Name = newCurrency.Name;

                context.SaveChanges();
            }
        }

        public void DeleteCurrency(int id)
        {
            using (var context = new MyDbContext())
            {
                var currency = FindCurrency(context, id);

                context.Currencies.Remove(currency);

                context.SaveChanges();
            }
        }
    }
}

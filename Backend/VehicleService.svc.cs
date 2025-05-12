using Backend.Db;
using Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Backend
{
    public class VehicleService : IVehicleService
    {
        static VehicleService()
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<MyDbContext>());

            using (var context = new MyDbContext())
            {
                context.Database.Initialize(false);
            }
        }

        public List<VehicleRead> GetVehicles()
        {
            using (var context = new MyDbContext())
            {
                return context.Vehicles.Join(context.Currencies,v => v.CurrencyId, c => c.Id,
                    (v, c) => new VehicleRead
                    {
                        Id = v.Id,
                        Brand = v.Brand,
                        Model = v.Model,
                        Price = v.Price,
                        Currency = c.Name
                    }).ToList();
            }
        }

        private Vehicle FindVehicle(MyDbContext context, int id)
        {
            var vehicle = context.Vehicles.Find(id);
            if (vehicle == null)
            {
                throw new Exception("Vehicle not found.");
            }
            return vehicle;
        }

        private int FindCurrencyId(MyDbContext context, string str)
        {
            int currencyId = context.Currencies.Where(c => c.Name == str).Select(c => c.Id).FirstOrDefault();
            if (currencyId == 0)
            {
                throw new Exception("Currency not found.");
            }
            return currencyId;
        }

        public VehicleRead GetVehicle(int id)
        {
            using (var context = new MyDbContext())
            {
                var vehicle = FindVehicle(context, id);

                return new VehicleRead
                {
                    Id = vehicle.Id,
                    Brand = vehicle.Brand,
                    Model = vehicle.Model,
                    Price = vehicle.Price,
                    Currency = context.Currencies.Find(vehicle.CurrencyId).Name
                };
            }
        }

        public void AddVehicle(VehicleCreate newVehicle)
        {
            using (var context = new MyDbContext())
            {
                int currencyId = FindCurrencyId(context, newVehicle.Currency);

                context.Vehicles.Add(new Vehicle
                {
                    Brand = newVehicle.Brand,
                    Model = newVehicle.Model,
                    Price = newVehicle.Price,
                    CurrencyId = currencyId
                });
                context.SaveChanges();
            }
        }

        public void UpdateVehicle(VehicleUpdate newVehicle)
        {
            using (var context = new MyDbContext())
            {
                var vehicle = FindVehicle(context, newVehicle.Id);
                int currencyId = FindCurrencyId(context, newVehicle.Currency);

                vehicle.Brand = newVehicle.Brand;
                vehicle.Model = newVehicle.Model;
                vehicle.Price = newVehicle.Price;
                vehicle.CurrencyId = currencyId;

                context.SaveChanges();
            }
        }

        public void DeleteVehicle(int id)
        {
            using (var context = new MyDbContext())
            {
                var vehicle = FindVehicle(context, id);

                context.Vehicles.Remove(vehicle);

                context.SaveChanges();
            }
        }
    }
}

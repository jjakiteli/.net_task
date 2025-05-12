using Frontend.CurrencyService;
using Frontend.VehicleService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Frontend.Controllers
{
    public class VehiclesController : Controller
    {
        private readonly VehicleServiceClient _client = new VehicleServiceClient();
        private readonly CurrencyServiceClient _currency_client = new CurrencyServiceClient();

        public ActionResult Index()
        {
            VehicleRead[] vehicles;
            try
            {
                vehicles = _client.GetVehicles();
            }
            catch (Exception ex)
            {
                return Content("Connection to backend lost.");
            }
            return View(vehicles);
        }

        public ActionResult AddVehicleForm()
        {
            var vehicle = new VehicleCreate
            {
                Currencies = _currency_client.GetCurrencies().Select(c => c.Name).ToArray()
            };
            return PartialView("AddVehicleForm", vehicle);
        }

        [HttpPost]
        public ActionResult AddVehicle(VehicleCreate vehicle)
        {
            if (!ModelState.IsValid)
                return PartialView("AddVehicle", vehicle);

            try
            {
                _client.AddVehicle(vehicle);
            }
            catch (Exception ex)
            {
                return PartialView("AddVehicle", vehicle);
            }

            return RedirectToAction("Index");
        }

        public ActionResult EditVehicleForm(int id)
        {
            VehicleRead vehicle;
            try
            {
                vehicle = _client.GetVehicle(id);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }

            var updateVehicle = new VehicleUpdate
            {
                Id = vehicle.Id,
                Brand = vehicle.Brand,
                Model = vehicle.Model,
                Price = vehicle.Price,
                Currency = vehicle.Currency,
                Currencies = _currency_client.GetCurrencies().Select(c => c.Name).ToArray()
            };
            return PartialView("EditVehicleForm", updateVehicle);
        }

        [HttpPost]
        public ActionResult EditVehicle(VehicleUpdate vehicle)
        {
            if (!ModelState.IsValid)
                return PartialView("EditVehicleForm", vehicle);

            try
            {
                _client.UpdateVehicle(vehicle);
            }
            catch (Exception ex)
            {
                return PartialView("EditVehicleForm", vehicle);
            }
            return RedirectToAction("Index");
        }

        public ActionResult DeleteVehicle(int id)
        {
            try
            {
                _client.DeleteVehicle(id);
            }
            catch (Exception ex)
            {
                RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        ~VehiclesController()
        {
            _client.Close();
        }
    }
}
using Frontend.CurrencyService;
using Frontend.VehicleService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace Frontend.Controllers
{
    public class VehiclesController : Controller
    {
        private readonly VehicleServiceClient _client = new VehicleServiceClient();
        private readonly CurrencyServiceClient _currency_client = new CurrencyServiceClient();
        private string[] CurrenciesArray => _currency_client.GetCurrencies().Select(c => c.Name).ToArray();

        public ActionResult Index()
        {
            VehicleRead[] vehicles;
            try
            {
                vehicles = _client.GetVehicles();
            }
            catch (Exception)
            {
                return RedirectToAction("ConnectionLost", "Index");
            }
            return View(vehicles);
        }

        public ActionResult AddVehicleForm()
        {
            var vehicle = new VehicleCreate
            {
                Currencies = CurrenciesArray
            };
            return PartialView("AddVehicleForm", vehicle);
        }

        [HttpPost]
        public JsonResult AddVehicle(VehicleCreate vehicle)
        {
            if (!ModelState.IsValid)
            {
                vehicle.Currencies = CurrenciesArray;
                return Json(new
                {
                    success = false,
                    html = RenderPartialViewToString("AddVehicleForm", vehicle),
                    message = "Wystąpił błąd walidacji danych. Proszę poprawić formularz."
                });
            }

            try
            {
                _client.AddVehicle(vehicle);
            }
            catch (Exception)
            {
                vehicle.Currencies = CurrenciesArray;
                return Json(new
                {
                    success = false,
                    html = RenderPartialViewToString("AddVehicleForm", vehicle),
                    message = "Wystąpił błąd podczas dodawania pojazdu. Spróbuj ponownie później."
                });
            }

            return Json(new { success = true });
        }

        public ActionResult EditVehicleForm(int id)
        {
            VehicleRead vehicle;
            try
            {
                vehicle = _client.GetVehicle(id);
            }
            catch (Exception)
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
                Currencies = CurrenciesArray
            };
            return PartialView("EditVehicleForm", updateVehicle);
        }

        [HttpPost]
        public JsonResult EditVehicle(VehicleUpdate vehicle)
        {
            if (!ModelState.IsValid)
            {
                vehicle.Currencies = CurrenciesArray;
                return Json(new
                {
                    success = false,
                    html = RenderPartialViewToString("EditVehicleForm", vehicle),
                    message = "Wystąpił błąd walidacji danych. Proszę poprawić formularz."
                });
            }

            try
            {
                _client.UpdateVehicle(vehicle);
            }
            catch (Exception)
            {
                vehicle.Currencies = CurrenciesArray;
                return Json(new
                {
                    success = false,
                    html = RenderPartialViewToString("EditVehicleForm", vehicle),
                    message = "Wystąpił błąd podczas aktualizacji pojazdu. Spróbuj ponownie później."
                });
            }

            return Json(new { success = true });
        }

        public ActionResult DeleteVehicleForm(int id)
        {
            VehicleRead vehicle;
            try
            {
                vehicle = _client.GetVehicle(id);
            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }
            return PartialView("DeleteVehicleForm", vehicle);
        }

        [HttpPost]
        public JsonResult DeleteVehicle(VehicleRead vehicle)
        {
            try
            {
                _client.DeleteVehicle(vehicle.Id);
            }
            catch (Exception)
            {
                return Json(new
                {
                    success = false,
                    html = RenderPartialViewToString("DeleteVehicleForm", vehicle),
                    message = "Wystąpił błąd podczas usuwania pojazdu. Spróbuj ponownie później."
                });
            }

            return Json(new { success = true });
        }

        ~VehiclesController()
        {
            _client.Close();
        }

        private string RenderPartialViewToString(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.RouteData.GetRequiredString("action");

            ViewData.Model = model;

            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }
    }
}
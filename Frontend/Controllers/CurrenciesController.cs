using Frontend.CurrencyService;
using System;
using System.IO;
using System.Web.Mvc;

namespace Frontend.Controllers
{
    public class CurrenciesController : Controller
    {
        private readonly ICurrencyService _client;

        public CurrenciesController(ICurrencyService currencyClient)
        {
            _client = currencyClient;
        }

        public ActionResult Index()
        {
            CurrencyRead[] currencies;
            try
            {
                currencies = _client.GetCurrencies();
            }
            catch (Exception)
            {
                return RedirectToAction("ConnectionLost", "Index");
            }
            return View(currencies);
        }

        public ActionResult AddCurrencyForm()
        {
            var currency = new CurrencyCreate();
            return PartialView("AddCurrencyForm", currency);
        }

        [HttpPost]
        public JsonResult AddCurrency(CurrencyCreate currency)
        {
            if (!ModelState.IsValid)
            {
                return Json(new
                {
                    success = false,
                    html = RenderPartialViewToString("AddCurrencyForm", currency),
                    message = "Wystąpił błąd walidacji danych. Proszę poprawić formularz."
                });
            }

            try
            {
                _client.AddCurrency(currency);
            }
            catch (Exception)
            {
                return Json(new
                {
                    success = false,
                    html = RenderPartialViewToString("AddCurrencyForm", currency),
                    message = "Wystąpił błąd podczas dodawania waluty. Spróbuj ponownie później."
                });
            }

            return Json(new { success = true });
        }

        public ActionResult EditCurrencyForm(int id)
        {
            CurrencyRead currency;
            try
            {
                currency = _client.GetCurrency(id);
            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }

            var updateCurrency = new CurrencyUpdate
            {
                Id = currency.Id,
                Name = currency.Name
            };
            return PartialView("EditCurrencyForm", updateCurrency);
        }

        [HttpPost]
        public JsonResult EditCurrency(CurrencyUpdate currency)
        {
            if (!ModelState.IsValid)
            {
                return Json(new
                {
                    success = false,
                    html = RenderPartialViewToString("EditCurrencyForm", currency),
                    message = "Wystąpił błąd walidacji danych. Proszę poprawić formularz."
                });
            }

            try
            {
                _client.UpdateCurrency(currency);
            }
            catch (Exception)
            {
                return Json(new
                {
                    success = false,
                    html = RenderPartialViewToString("EditCurrencyForm", currency),
                    message = "Wystąpił błąd podczas aktualizacji waluty. Spróbuj ponownie później."
                });
            }

            return Json(new { success = true });
        }

        public ActionResult DeleteCurrencyForm(int id)
        {
            CurrencyRead currency;
            try
            {
                currency = _client.GetCurrency(id);
            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }
            return PartialView("DeleteCurrencyForm", currency);
        }

        [HttpPost]
        public JsonResult DeleteCurrency(CurrencyRead currency)
        {
            try
            {
                _client.DeleteCurrency(currency.Id);
            }
            catch (Exception)
            {
                return Json(new
                {
                    success = false,
                    html = RenderPartialViewToString("DeleteCurrencyForm", currency),
                    message = "Wystąpił błąd podczas usuwania waluty. Spróbuj ponownie później."
                });
            }

            return Json(new { success = true });
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
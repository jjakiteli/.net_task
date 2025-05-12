using Frontend.CurrencyService;
using System;
using System.Web.Mvc;

namespace Frontend.Controllers
{
    public class CurrenciesController : Controller
    {
        private readonly CurrencyServiceClient _client = new CurrencyServiceClient();

        public ActionResult Index()
        {
            CurrencyRead[] currencies;
            try
            {
                currencies = _client.GetCurrencies();
            }
            catch (Exception ex)
            {
                return Content("Connection to backend lost.");
            }
            return View(currencies);
        }

        public ActionResult AddCurrencyForm()
        {
            var currency = new CurrencyCreate();
            return PartialView("AddCurrencyForm", currency);
        }

        [HttpPost]
        public ActionResult AddCurrency(CurrencyCreate currency)
        {
            if (!ModelState.IsValid)
                return PartialView("AddCurrency", currency);

            try
            {
                _client.AddCurrency(currency);
            }
            catch (Exception ex)
            {
                return PartialView("AddCurrency", currency);
            }

            return RedirectToAction("Index");
        }

        public ActionResult EditCurrencyForm(int id)
        {
            CurrencyRead currency;
            try
            {
                currency = _client.GetCurrency(id);
            }
            catch (Exception ex)
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
        public ActionResult EditCurrency(CurrencyUpdate currency)
        {
            if (!ModelState.IsValid)
                return PartialView("EditCurrencyForm", currency);

            try
            {
                _client.UpdateCurrency(currency);
            }
            catch (Exception ex)
            {
                return PartialView("EditCurrencyForm", currency);
            }
            return RedirectToAction("Index");
        }

        public ActionResult DeleteCurrency(int id)
        {
            try
            {
                _client.DeleteCurrency(id);
            }
            catch (Exception ex)
            {
                RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        ~CurrenciesController()
        {
            _client.Close();
        }
    }
}
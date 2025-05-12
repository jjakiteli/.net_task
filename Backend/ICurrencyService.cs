using Models;
using System.Collections.Generic;
using System.ServiceModel;

namespace Backend
{
    [ServiceContract]
    public interface ICurrencyService
    {
        [OperationContract]
        List<CurrencyRead> GetCurrencies();

        [OperationContract]
        CurrencyRead GetCurrency(int id);

        [OperationContract]
        void AddCurrency(CurrencyCreate newCurrency);

        [OperationContract]
        void UpdateCurrency(CurrencyUpdate newCurrency);

        [OperationContract]
        void DeleteCurrency(int id);
    }
}

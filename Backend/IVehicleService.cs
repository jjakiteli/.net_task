using Models;
using System.Collections.Generic;
using System.ServiceModel;

namespace Backend
{
    [ServiceContract]
    public interface IVehicleService
    {
        [OperationContract]
        List<VehicleRead> GetVehicles();

        [OperationContract]
        VehicleRead GetVehicle(int id);

        [OperationContract]
        void AddVehicle(VehicleCreate newVehicle);

        [OperationContract]
        void UpdateVehicle(VehicleUpdate newVehicle);

        [OperationContract]
        void DeleteVehicle(int id);
    }
}

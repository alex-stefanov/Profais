using Profais.Data.Models;
using Profais.Data.Repositories;
using Profais.Services.Interfaces;

namespace Profais.Services.Implementations;

public class VehicleService(
    IRepository<Vehicle,int> vehicleRepository)
    : IVehicleService
{

}

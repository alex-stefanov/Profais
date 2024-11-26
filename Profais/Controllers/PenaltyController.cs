using Microsoft.AspNetCore.Mvc;
using Profais.Services.Interfaces;

namespace Profais.Controllers;

public class PenaltyController(
    IPenaltyService penaltyService,
    ILogger<PenaltyController> logger)
    : Controller
{

}
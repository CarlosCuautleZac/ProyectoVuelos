using AerolineaApi.DTOs;
using AerolineaApi.Models;
using AerolineaApi.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AerolineaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ObservacionController : ControllerBase
    {
        Repository<Observacion> repository;
        public ObservacionController(sistem21_aerolineaContext context)
        {
            repository = new(context);
        }

        [HttpGet]
        public IActionResult Get()
        {
            var obervaciones = repository.Get().Select(x => new ObservacionDTO()
            {
                Id = x.Id,
                Observacion1 = x.Observacion1
            });

            return Ok(obervaciones);

        }
    }

}

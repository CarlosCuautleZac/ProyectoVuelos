using AerolineaApi.DTOs;
using AerolineaApi.Models;
using AerolineaApi.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AerolineaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VueloController : ControllerBase
    {
        private readonly sistem21_aerolineaContext context;
        Repository<Vuelo> repository;
        Repository<Observacion> repositoryobservacion;

        public VueloController(sistem21_aerolineaContext context)
        {
            this.context = context;
            repository = new(context);
            repositoryobservacion = new(context);
        }

        [HttpGet]
        public IActionResult Get()
        {
            Limpiar();

            var vuelos = repository.Get().Include(x => x.IdobservacionNavigation).Select(x => new VueloDTO
            {
                Id = x.Id,
                Aerolinea = x.Aerolinea,
                Destino = x.Destino,
                Fecha = x.Fecha,
                Observacion = x.IdobservacionNavigation.Observacion1,
                Puerta = x.Puerta
            }).OrderBy(x => x.Fecha).ToList();



            return Ok(vuelos);
        }

        private void Limpiar()
        {
            DateTime ahora = DateTime.Now.AddHours(2);

            //Limpiamos los cancelados
            var v = repository.Get().Include(x => x.IdobservacionNavigation).
                 Where(x => x.IdobservacionNavigation.Observacion1 == "Cancelado"
                 || x.IdobservacionNavigation.Observacion1 == "Abordando").ToList();

            if (v.Count > 0)
            {
                for (int i = 0; i < v.Count(); i++)
                {
                    var h = v[i].FechaModificacion;
                    if (h != null)
                    {
                        var hourtoeliminate = h.Value.AddMinutes(2).AddHours(2);

                        if (hourtoeliminate < ahora.AddHours(2))
                            repository.Delete(v[i]);
                    }



                }
            }

            //cambiamos el estado a los vuelos que estan atrasados
            var retrasados = repository.Get().Include(x => x.IdobservacionNavigation).
                Where(x => x.IdobservacionNavigation.Observacion1 != "Cancelado"
                &&x.IdobservacionNavigation.Observacion1!= "Abordando"&& x.IdobservacionNavigation.Observacion1 != "Atrasado" &&
                x.Fecha < ahora).ToList();
            if (retrasados.Count > 0)
            {
                for (int i = 0; i < retrasados.Count(); i++)
                {
                    var vuelo = retrasados[i];
                    var vueloencontrado = repository.Get(vuelo.Id);
                    if (vueloencontrado != null)
                    {
                        vueloencontrado.Idobservacion = 3;
                        repository.Update(vueloencontrado);
                    }
                }
            }

        }

        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {


            var vuelos = repository.Get().Include(x => x.IdobservacionNavigation).Where(x => x.Id == id).Select(x => new VueloDTO
            {
                Id = x.Id,
                Aerolinea = x.Aerolinea,
                Destino = x.Destino,
                Fecha = x.Fecha,
                Observacion = x.IdobservacionNavigation.Observacion1,
                Puerta = x.Puerta
            }).OrderBy(x => x.Fecha).FirstOrDefault();

            if (vuelos == null)
                return NotFound("No se encontro el vuelo");
            else
            {
                return Ok(vuelos);
            }

        }



        [HttpPost]
        public IActionResult Post(VueloDTO vuelo)
        {
            if (vuelo == null)
                return BadRequest("Debe proporcionar un vuelo");

            if (Validate(vuelo, out List<string> errores))
            {
                Vuelo v = new()
                {
                    Aerolinea = vuelo.Aerolinea.Trim().ToUpper(),
                    Destino = vuelo.Destino.Trim().ToUpper(),
                    Fecha = vuelo.Fecha,
                    Idobservacion = repositoryobservacion.Get().Where(x => x.Observacion1 == vuelo.Observacion)
                    .Select(x => x.Id).FirstOrDefault(),
                    Puerta = vuelo.Puerta,
                    FechaModificacion = DateTime.Now.AddHours(2)
                };

                repository.Insert(v);
                return Ok();
            }
            else
                return BadRequest(errores);

        }

        [HttpPut]
        public IActionResult Put(VueloDTO vuelo)
        {
            if (vuelo == null)
                return BadRequest("Envie la informacion correctamente");

            var v = repository.Get(vuelo.Id);

            if (v == null)
                return NotFound("No se encontro el vuelo a editar");

            if (Validate(vuelo, out List<string> errores))
            {
                if (vuelo.Fecha < v.Fecha)
                {
                    errores.Add("La fecha a modificar no puede ser menor a la establecida. Escriba una igual o mayor e " +
                        "intente hacer otra solicitud");
                    return BadRequest(errores);
                }

                v.Aerolinea = vuelo.Aerolinea.Trim().ToUpper();
                v.Destino = vuelo.Destino.Trim().ToUpper();
                v.Fecha = vuelo.Fecha;
                v.Idobservacion = repositoryobservacion.Get().Where(x => x.Observacion1 == vuelo.Observacion)
                .Select(x => x.Id).FirstOrDefault();
                v.Puerta = vuelo.Puerta;
                v.FechaModificacion = DateTime.Now.AddHours(2);


                repository.Update(v);
                return Ok();
            }
            else
                return BadRequest(errores);
        }

        [HttpDelete]
        public IActionResult Delete(VueloDTO vuelo)
        {
            var v = repository.Get(vuelo.Id);

            if (v == null)
                return NotFound();

            repository.Delete(v);
            return Ok();
        }

        private bool Validate(VueloDTO vuelo, out List<string> errors)
        {
            errors = new();

            if (string.IsNullOrWhiteSpace(vuelo.Destino))
                errors.Add("Ingrese un destino e intente hacer otra solicitud");

            if (string.IsNullOrWhiteSpace(vuelo.Aerolinea))
                errors.Add("Debe ingresar una aerolinea.");

            if (vuelo.Id == 0)
            {
                if (vuelo.Fecha < DateTime.Now)
                    errors.Add("Fecha invalida. Debe escribir una fecha correcta para contiuar");
            }
              
            //la puerta puede ser nula
            //if (vuelo.Puerta < 1 || vuelo.Puerta > 20)
            //    errors.Add("Escriba una puerta entre la número 1 y la número 20");

            //ver como se comporta -- recordatorio
            if (vuelo.Puerta > 0)
                if (repository.Get().Any(x => x.Puerta == vuelo.Puerta && x.Id != vuelo.Id))
                    errors.Add("Ya se esta ocupando esa puerta, escriba otra para e intente de nuevo");


            return errors.Count == 0;

        }
    }
}

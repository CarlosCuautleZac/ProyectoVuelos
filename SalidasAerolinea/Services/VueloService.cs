using Newtonsoft.Json;
using SalidasAerolinea.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SalidasAerolinea.Services
{
    public class VueloService
    {
        HttpClient client = new HttpClient()
        {
            BaseAddress = new Uri("https://aerolinea.sistemas19.com/api/vuelo")
        };

        public async Task<List<Vuelo>> Get()
        {
            List<Vuelo>? vuelos = new List<Vuelo>();
            var response = await client.GetAsync("");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                vuelos = JsonConvert.DeserializeObject<List<Vuelo>>(json);
            }

            if (vuelos == null)
                return  new List<Vuelo>();
            else
                return vuelos;

        }

    }
}

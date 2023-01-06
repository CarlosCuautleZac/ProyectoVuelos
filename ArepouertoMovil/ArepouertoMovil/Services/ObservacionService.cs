using ArepouertoMovil.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ArepouertoMovil.Services
{
    public class ObservacionService
    {
        HttpClient client;

        public ObservacionService()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://aerolinea.sistemas19.com/api/observacion");
        }

        public async Task<List<Observacion>> Get()
        {
            try
            {
                List<Observacion> observaciones = new List<Observacion>();
                var response = client.GetAsync("");
                response.Wait();
                if (response.Result.IsSuccessStatusCode)
                {
                    var json = await response.Result.Content.ReadAsStringAsync();
                    observaciones = JsonConvert.DeserializeObject<List<Observacion>>(json);
                }

                if (observaciones == null)
                    return new List<Observacion>();
                else
                    return observaciones;
            }
            catch (Exception m)
            {
                var error = m.Message;
                return new List<Observacion>();
            }
        } 
    }
}

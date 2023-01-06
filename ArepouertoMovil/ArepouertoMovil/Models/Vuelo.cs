using System;
using System.Collections.Generic;
using System.Text;

namespace ArepouertoMovil.Models
{
    public class Vuelo
    {
        public int Id { get; set; }
        public string Destino { get; set; }
        public string Aerolinea { get; set; }
        public DateTime Fecha { get; set; }
        public int? Puerta { get; set; }
        public string Observacion { get; set; }
        
    }
}

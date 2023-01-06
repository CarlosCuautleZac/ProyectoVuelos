using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalidasAerolinea.Models
{
    public class Vuelo
    {
        public int Id { get; set; }
        public string Destino { get; set; } = null!;
        public string Aerolinea { get; set; } = null!;
        public DateTime Fecha { get; set; }
        public int? Puerta { get; set; }
        public string Observacion { get; set; } = "";
    }
}

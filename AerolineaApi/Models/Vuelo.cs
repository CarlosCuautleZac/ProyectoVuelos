using System;
using System.Collections.Generic;

namespace AerolineaApi.Models
{
    public partial class Vuelo
    {
        public int Id { get; set; }
        public string Destino { get; set; } = null!;
        public string Aerolinea { get; set; } = null!;
        public DateTime Fecha { get; set; }
        public int? Puerta { get; set; }
        public int Idobservacion { get; set; }
        public DateTime? FechaModificacion { get; set; }

        public virtual Observacion IdobservacionNavigation { get; set; } = null!;
    }
}

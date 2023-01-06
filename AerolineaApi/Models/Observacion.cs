using System;
using System.Collections.Generic;

namespace AerolineaApi.Models
{
    public partial class Observacion
    {
        public Observacion()
        {
            Vuelo = new HashSet<Vuelo>();
        }

        public int Id { get; set; }
        public string Observacion1 { get; set; } = null!;

        public virtual ICollection<Vuelo> Vuelo { get; set; }
    }
}

namespace AerolineaApi.DTOs
{
    public class VueloDTO
    {
        public int Id { get; set; }
        public string Destino { get; set; } = null!;
        public string Aerolinea { get; set; } = null!;
        public DateTime Fecha { get; set; }
        public int? Puerta { get; set; }
        public string Observacion { get; set; } = "";
        
    }
}

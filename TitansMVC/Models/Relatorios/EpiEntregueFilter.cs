namespace TitansMVC.Models.Relatorios
{
    public class EpiEntregueFilter
    {
        public int ColaboradorId { get; set; }
        public int EpiId { get; set; }
        public int SetorId { get; set; }
        public int CentroCustoId { get; set; }
        public string TipoRelatorio { get; set; }
    }
}
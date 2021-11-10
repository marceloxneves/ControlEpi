namespace TitansMVC.Models.Relatorios
{
    public class UniformeEntregueFilter
    {
        public int ColaboradorId { get; set; }
        public int UniformeId { get; set; }
        public int SetorId { get; set; }
        public int CentroCustoId { get; set; }
        public string TipoRelatorio { get; set; }
    }
}
using System;
using System.ComponentModel;
using TitansMVC.Models.Enums;

namespace TitansMVC.Models.Relatorios
{
    public class ColaboradorEpiFilter
    {
        public int? UnidadeNegocioId { get; set; }
        public int? ColaboradorId { get; set; }
        public int? EpiId { get; set; }
        public int? TipoEpiId { get; set; }
        public string Marca { get; set; }
        public int? SetorId { get; set; }
        public int? CentroCustoId { get; set; }
        [DisplayName("Ativos?")]
        public bool Ativos { get; set; }
        public DateTime? DataInicial { get; set; }
        public DateTime? DataFinal { get; set; }
        public EstadoEpisConsulta EstadoEpis { get; set; }
        public string TipoRelatorio { get; set; }        

    }
}
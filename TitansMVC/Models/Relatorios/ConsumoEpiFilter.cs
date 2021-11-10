using System;
using System.ComponentModel.DataAnnotations;
using TitansMVC.Models.Enums;
using TitansMVC.Properties;

namespace TitansMVC.Models.Relatorios
{
    public class ConsumoEpiFilter
    {
        public int? UnidadeNegocioId { get; set; }
        public int? ColaboradorId { get; set; }     
        public int? EpiId { get; set; }
        public int? TipoEpiId { get; set; }
        public string Marca { get; set; }
        public int? SetorId { get; set; }
        public int? CentroCustoId { get; set; }
        public DateTime? DataInicial { get; set; }
        public DateTime? DataFinal { get; set; }
        public string TipoRelatorio { get; set; }
        public decimal? Custo { get; set; }

    }
}
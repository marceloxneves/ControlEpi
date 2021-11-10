using System;
using System.ComponentModel;
using TitansMVC.Models.Enums;

namespace TitansMVC.Models.Relatorios
{
    public class ColaboradorUniformeFilter
    {
        public int? UnidadeNegocioId { get; set; }
        public int? ColaboradorId { get; set; }
        public int? UniformeId { get; set; }
        public int? TipoUniformeId { get; set; }
        public string Marca { get; set; }
        public int? SetorId { get; set; }
        public int? CentroCustoId { get; set; }
        [DisplayName("Ativos?")]
        public bool Ativos { get; set; }
        public DateTime? DataInicial { get; set; }
        public DateTime? DataFinal { get; set; }
        public EstadoUniformesConsulta EstadoUniformes { get; set; }
        public string TipoRelatorio { get; set; }        

    }
}
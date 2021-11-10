using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TitansMVC.Models
{
    public class EstoqueEpi
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Empresa")]
        public int IdEmpresa { get; set; }

        [DisplayName("Epi")]
        public int IdEpi { get; set; }
        public virtual EpiModel Epi { get; set; }

        [DisplayName("Quantidade")]
        public decimal Qtde { get; set; }

        [DisplayName("Quantidade Máxima")]
        public decimal QtdeMax { get; set; }

        [DisplayName("Quantidade Mínima")]
        public decimal QtdeMin { get; set; }

        [ScaffoldColumn(false)]
        public DateTime? DataCad { get; set; }

        public void AtualizarEstoque(decimal qtdeAdicionada)
        {
            this.Qtde = this.Qtde + qtdeAdicionada;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using TitansMVC.Models;

namespace TitansMVC.EntityConfiguration
{
    public class EpiConfiguration: EntityTypeConfiguration<EpiModel>
    {
        public EpiConfiguration()
        {
            ToTable("epi");
            HasKey(e => e.Id);

            Property(e => e.Id).HasColumnName("id");
            Property(e => e.IdEmpresa).HasColumnName("id_empresa");
            Property(e => e.Nome).HasColumnName("nome");
            Property(e => e.Marca).HasColumnName("marca").IsOptional();
            Property(e => e.Ca).HasColumnName("ca").IsOptional();
            Property(e => e.ValidadeCa).HasColumnName("validade_ca").IsOptional();
            Property(e => e.CodEpiFabricante).HasColumnName("cod_epi_fabricante").IsOptional();
            Property(e => e.CodEpiCliente).HasColumnName("cod_epi_cliente").IsOptional();
            Property(e => e.CodEpiTitans).HasColumnName("cod_epi_titans").IsOptional();
            Property(e => e.TipoEpiId).HasColumnName("id_tipo_epi");
            //familia possivelmente tirar
            Property(e => e.FamiliaId).HasColumnName("id_familia").IsOptional();
            Property(e => e.Custo).HasColumnName("custo").IsOptional();
            Property(e => e.Ativo).HasColumnName("ativo").IsOptional();
            Property(e => e.Obs).HasColumnName("obs").HasMaxLength(500).IsOptional();
            Property(e => e.Foto).HasColumnName("foto").IsOptional();
            Property(e => e.Importado).HasColumnName("importado").IsOptional();
            Property(c => c.UnidadeNegocioId).HasColumnName("id_unidade_negocio");
            Property(c => c.CodigoDeBarras).HasColumnName("codigo_de_barras");

            //Ignore(e => e.DataCad);
            Property(e => e.DataCad).HasColumnName("data_cadastro").IsOptional();

            HasRequired(e => e.Empresa).WithMany().HasForeignKey(e => e.IdEmpresa);
            HasRequired(c => c.UnidadeNegocio).WithMany().HasForeignKey(c => c.UnidadeNegocioId);
            //HasMany(e => e.Estoques).WithRequired(a => a.Epi).HasForeignKey(a => a.IdEpi).WillCascadeOnDelete(true);
            HasMany<EstoqueEpi>(e => e.Estoques).WithRequired(a => a.Epi).HasForeignKey(a => a.IdEpi).WillCascadeOnDelete(true);


            //public int FamiliaId { get; set; }
            //public FamiliaEpiModel Familia { get; set; }
            //public decimal? Custo { get; set; }
            //public int EmpresaId { get; set; }
            //public EmpresaModel Empresa { get; set; }
            //public string Obs { get; set; }
            //public DateTime DataCadastro { get; set; }
        }
    }
}
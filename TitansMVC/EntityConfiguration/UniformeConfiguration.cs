using System.Data.Entity.ModelConfiguration;
using TitansMVC.Models;

namespace TitansMVC.EntityConfiguration
{
    public class UniformeConfiguration: EntityTypeConfiguration<UniformeModel>
    {
        public UniformeConfiguration()
        {
            ToTable("uniforme");
            HasKey(e => e.Id);

            Property(e => e.Id).HasColumnName("id");
            Property(e => e.IdEmpresa).HasColumnName("id_empresa");
            Property(e => e.Nome).HasColumnName("nome");
            Property(e => e.Marca).HasColumnName("marca").IsOptional();
            Property(e => e.CodUniformeFabricante).HasColumnName("cod_fabricante").IsOptional();
            Property(e => e.CodUniformeCliente).HasColumnName("cod_cliente").IsOptional();
            Property(e => e.CodUniformeTitans).HasColumnName("cod_uniforme_titans").IsOptional();
            Property(e => e.TipoUniformeId).HasColumnName("id_tipo_uniforme");
            Property(e => e.Custo).HasColumnName("custo").IsOptional();
            Property(e => e.Ativo).HasColumnName("ativo").IsOptional();
            Property(e => e.Obs).HasColumnName("obs").HasMaxLength(500).IsOptional();
            Property(e => e.Foto).HasColumnName("foto").IsOptional();
            Property(e => e.Importado).HasColumnName("importado").IsOptional();
            Property(c => c.UnidadeNegocioId).HasColumnName("id_unidade_negocio");
            Property(c => c.CodigoDeBarras).HasColumnName("codigo_de_barras");

            Property(e => e.DataCad).HasColumnName("data_cadastro").IsOptional();

            HasRequired(e => e.Empresa).WithMany().HasForeignKey(e => e.IdEmpresa);
            HasRequired(c => c.UnidadeNegocio).WithMany().HasForeignKey(c => c.UnidadeNegocioId);
            HasMany<EstoqueUniforme>(e => e.Estoques).WithRequired(a => a.Uniforme).HasForeignKey(a => a.IdUniforme).WillCascadeOnDelete(true);
       
        }
    }
}
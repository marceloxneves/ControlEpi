using System.Data.Entity.ModelConfiguration;
using TitansMVC.Models;

namespace TitansMVC.EntityConfiguration
{
    public class EstoqueUniformeConfiguration:EntityTypeConfiguration<EstoqueUniforme>
    {
        public EstoqueUniformeConfiguration()
        {
            ToTable("estoque_uniforme");
            HasKey(e => e.Id);

            Property(e => e.Id).HasColumnName("id");
            Property(e => e.IdEmpresa).HasColumnName("id_empresa");
            Property(e => e.IdUniforme).HasColumnName("id_uniforme").IsRequired();
            Property(e => e.Qtde).HasColumnName("qtde").IsRequired();
            Property(e => e.QtdeMax).HasColumnName("qtde_max").IsOptional();
            Property(e => e.QtdeMin).HasColumnName("qtde_min").IsOptional();
            Property(e => e.DataCad).HasColumnName("data_cad").IsOptional();

            HasRequired(e => e.Uniforme).WithMany().HasForeignKey(e => e.IdUniforme);
        }
    }
}

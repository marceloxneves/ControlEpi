using System.Data.Entity.ModelConfiguration;
using TitansMVC.Models;

namespace TitansMVC.EntityConfiguration
{
    public class CentroCustoConfiguration : EntityTypeConfiguration<CentroCustoModel>
    {
        public CentroCustoConfiguration()
        {
            ToTable("centro_custo");
            HasKey(c => c.Id);

            Property(c => c.Id).HasColumnName("id");
            Property(c => c.IdEmpresa).HasColumnName("id_empresa");
            Property(c => c.Nome).HasColumnName("nome");
            Property(c => c.LbcId).HasColumnName("id_lbc").IsOptional();
            Property(c => c.Ativo).HasColumnName("ativo").IsOptional();
            Property(c => c.Obs).HasColumnName("obs").HasMaxLength(500).IsOptional();

            //Ignore(c => c.DataCad);
            Property(c => c.DataCad).HasColumnName("data_cad").IsOptional();

            HasRequired(c => c.Empresa).WithMany().HasForeignKey(c => c.IdEmpresa);
            HasOptional(c => c.Lbc).WithMany().HasForeignKey(c => c.LbcId);
        }
    }
}
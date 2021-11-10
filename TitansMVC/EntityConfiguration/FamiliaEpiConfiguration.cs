using System.Data.Entity.ModelConfiguration;
using TitansMVC.Models;

namespace TitansMVC.EntityConfiguration
{
    public class FamiliaEpiConfiguration : EntityTypeConfiguration<FamiliaEpiModel>
    {
        public FamiliaEpiConfiguration()
        {
            ToTable("familia_epi");
            HasKey(f => f.Id);

            Property(f => f.Id).HasColumnName("id");
            Property(f => f.IdEmpresa).HasColumnName("id_empresa");
            Property(f => f.Nome).HasColumnName("nome");
            Property(f => f.Ativo).HasColumnName("ativo").IsOptional();
            Property(f => f.Obs).HasColumnName("obs").HasMaxLength(500).IsOptional();

            //Ignore(f => f.DataCad);
            Property(f => f.DataCad).HasColumnName("data_cad").IsOptional();

            HasRequired(f => f.Empresa).WithMany().HasForeignKey(f => f.IdEmpresa);

            //HasRequired(s => s.Empresa).WithMany().HasForeignKey(s => s.EmpresaId);
        }
    }
}
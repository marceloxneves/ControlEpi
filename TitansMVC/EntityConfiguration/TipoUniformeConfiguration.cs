using System.Data.Entity.ModelConfiguration;
using TitansMVC.Models;

namespace TitansMVC.EntityConfiguration
{
    public class TipoUniformeConfiguration : EntityTypeConfiguration<TipoUniformeModel>
    {
        public TipoUniformeConfiguration()
        {
            ToTable("tipo_uniforme");
            HasKey(s => s.Id);

            Property(s => s.Id).HasColumnName("id");
            Property(s => s.IdEmpresa).HasColumnName("id_empresa");
            Property(s => s.Nome).HasColumnName("nome");
            Property(s => s.Ativo).HasColumnName("ativo").IsOptional();
            Property(s => s.Obs).HasColumnName("obs").HasMaxLength(500).IsOptional();

            HasRequired(s => s.Empresa).WithMany().HasForeignKey(s => s.IdEmpresa);
        }
    }
}
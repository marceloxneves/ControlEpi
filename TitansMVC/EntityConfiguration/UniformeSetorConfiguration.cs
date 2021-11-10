using System.Data.Entity.ModelConfiguration;
using TitansMVC.Models;

namespace TitansMVC.EntityConfiguration
{
    public class UniformeSetorConfiguration: EntityTypeConfiguration<UniformeSetorModel>
    {
        public UniformeSetorConfiguration()
        {
            ToTable("setor_uniforme");
            Property(e => e.Id).HasColumnName("id");
            Property(e => e.TipoUniformeId).HasColumnName("id_tipo_uniforme");
            Property(e => e.SetorId).HasColumnName("id_setor");
            Property(e => e.UniformeId).HasColumnName("id_uniforme");
            Property(e => e.NomeUniforme).HasColumnName("nome_uniforme").IsOptional();
            Property(e => e.ValidadeEmDias).HasColumnName("validade_dias");
            Ignore(e => e.DescricaoSelectList);

            HasRequired(e => e.Setor).WithMany().HasForeignKey(e => e.SetorId);
            HasRequired(e => e.Uniforme).WithMany().HasForeignKey(e => e.UniformeId);
        }
    }
}
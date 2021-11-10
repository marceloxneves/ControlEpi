using System.Data.Entity.ModelConfiguration;
using TitansMVC.Models;

namespace TitansMVC.EntityConfiguration
{
    public class FichaUniformeConfiguration : EntityTypeConfiguration<FichaUniformeModel>
    {
        public FichaUniformeConfiguration()
        {
            ToTable("ficha_uniforme");
            HasKey(f => f.Id);

            Property(f => f.IdUniforme).HasColumnName("id_uniforme");
            Property(f => f.UniformeNome).HasColumnName("uniforme_nome").IsOptional();
            Property(f => f.RevisadoEm).HasColumnName("revisado_em").IsOptional();
            Property(f => f.DescrDetEquip).HasColumnName("descricao_det_equip").HasMaxLength(500).IsOptional();
            Property(f => f.FornFabr).HasColumnName("fornecedor_fabricante").HasMaxLength(500).IsOptional();
            Property(f => f.FormaHigienizacao).HasColumnName("forma_higienizacao").HasMaxLength(500).IsOptional();
            Property(f => f.FinalidadeAreaAplic).HasColumnName("finalidade_area_aplic").HasMaxLength(500).IsOptional();
            Property(f => f.AprovadoPor).HasColumnName("aprovado_por").IsOptional();
            Property(f => f.Registro).HasColumnName("registro").HasMaxLength(50).IsOptional();
            Property(f => f.Area).HasColumnName("area").IsOptional();
            Property(f => f.PecasReposicao).HasColumnName("pecas_reposicao").HasMaxLength(500).IsOptional();
            Property(f => f.Obs).HasColumnName("obs").HasMaxLength(500).IsOptional();
            Property(f => f.NumIdentificacao).HasColumnName("num_identificacao").HasMaxLength(500).IsOptional();

            Ignore(f => f.Uniforme);
        }
    }
}
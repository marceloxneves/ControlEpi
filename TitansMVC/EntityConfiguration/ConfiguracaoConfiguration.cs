using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using TitansMVC.Models;

namespace TitansMVC.EntityConfiguration
{
    public class ConfiguracaoConfiguration: EntityTypeConfiguration<ConfiguracaoModel>
    {
        public ConfiguracaoConfiguration()
        {
            ToTable("configuracao");
            HasKey(c => c.Id);

            Property(c => c.Id).HasColumnName("id");
            Property(c => c.IdEmpresa).HasColumnName("id_empresa");
            Property(c => c.AvisarVencCaComAntec).HasColumnName("avisar_antec_venc_ca").IsOptional();
            Property(c => c.QtdeDiasAvisoVencCa).HasColumnName("qtde_dias_aviso_venc_ca").IsOptional();
            Property(c => c.AvisarVencEpiComAntec).HasColumnName("avisar_antec_venc_epi").IsOptional();
            Property(c => c.QtdeDiasAvisoVencEpi).HasColumnName("qtde_dias_aviso_venc_epi").IsOptional();
            Property(c => c.AvisarAposVencCa).HasColumnName("avisar_apos_venc_ca").IsOptional();
            Property(c => c.AvisarAposVencEpi).HasColumnName("avisar_apos_venc_epi").IsOptional();
            Property(c => c.BloquearPorTipoEpiUnico).HasColumnName("tipo_epi_unico").IsOptional();
            Property(c => c.Obs).HasColumnName("obs").HasMaxLength(500).IsOptional();
            Property(c => c.AvisarVencUniformeComAntec).HasColumnName("avisar_antec_venc_uniforme").IsOptional();
            Property(c => c.QtdeDiasAvisoVencUniforme).HasColumnName("qtde_dias_aviso_venc_uniforme").IsOptional();
            Property(c => c.AvisarAposVencUniforme).HasColumnName("avisar_apos_venc_uniforme").IsOptional();
            Property(c => c.BloquearPorTipoUniformeUnico).HasColumnName("tipo_uniforme_unico").IsOptional();

            HasRequired(c => c.Empresa).WithMany().HasForeignKey(c => c.IdEmpresa);
        }
    }
}
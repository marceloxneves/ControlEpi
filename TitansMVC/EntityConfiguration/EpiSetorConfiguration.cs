using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using TitansMVC.Models;

namespace TitansMVC.EntityConfiguration
{
    public class EpiSetorConfiguration: EntityTypeConfiguration<EpiSetorModel>
    {
        public EpiSetorConfiguration()
        {
            ToTable("setor_epi");
            Property(e => e.Id).HasColumnName("id");
            Property(e => e.TipoEpiId).HasColumnName("id_tipo_epi");
            Property(e => e.SetorId).HasColumnName("id_setor");
            //Property(e => e.SetorNome).HasColumnName("nome_setor").IsOptional();
            Property(e => e.EpiId).HasColumnName("id_epi");
            Property(e => e.NomeEpi).HasColumnName("nome_epi").IsOptional();
            Property(e => e.ValidadeEmDias).HasColumnName("validade_dias");
            Ignore(e => e.DescricaoSelectList);

            HasRequired(e => e.Setor).WithMany().HasForeignKey(e => e.SetorId);
            HasRequired(e => e.Epi).WithMany().HasForeignKey(e => e.EpiId);
        }
    }
}
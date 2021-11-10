using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using TitansMVC.Models;

namespace TitansMVC.EntityConfiguration
{
    public class EmailEmpresaConfiguration : EntityTypeConfiguration<EmailEmpresaModel>
    {
        public EmailEmpresaConfiguration()
        {
            ToTable("empresa_email");
            HasKey(e => e.Id);
            Property(e => e.Id).HasColumnName("id");
            Property(e => e.IdEmpresa).HasColumnName("id_empresa").IsRequired();
            Property(e => e.Descricao).HasColumnName("descricao").HasMaxLength(50).IsOptional();
            Property(e => e.Email).HasColumnName("email").IsRequired();

            HasRequired(c => c.Empresa).WithMany().HasForeignKey(c => c.IdEmpresa);
        }
    }
}
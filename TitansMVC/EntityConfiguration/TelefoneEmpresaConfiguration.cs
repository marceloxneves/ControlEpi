using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using TitansMVC.Models;

namespace TitansMVC.EntityConfiguration
{
    public class TelefoneEmpresaConfiguration : EntityTypeConfiguration<TelefoneEmpresaModel>
    {
        public TelefoneEmpresaConfiguration()
        {
            ToTable("empresa_telefone");
            HasKey(t => t.Id);
            Property(t => t.Id).HasColumnName("id");
            Property(t => t.IdEmpresa).HasColumnName("id_empresa").IsRequired();
            Property(t => t.Numero).HasColumnName("tel_numero").HasMaxLength(20);
            Property(t => t.Descricao).HasColumnName("tel_descricao").HasMaxLength(50);
            Property(t => t.Ramal).HasColumnName("tel_ramal").HasMaxLength(20).IsOptional();

            HasRequired(t => t.Empresa).WithMany().HasForeignKey(t => t.IdEmpresa).WillCascadeOnDelete(true);
        }
    }
}
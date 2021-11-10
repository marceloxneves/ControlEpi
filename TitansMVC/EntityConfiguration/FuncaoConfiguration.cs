using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using TitansMVC.Models;

namespace TitansMVC.EntityConfiguration
{
    public class FuncaoConfiguration : EntityTypeConfiguration<FuncaoModel>
    {
        public FuncaoConfiguration()
        {
            ToTable("funcao");
            HasKey(s => s.Id);

            Property(s => s.Id).HasColumnName("id");
            Property(s => s.IdEmpresa).HasColumnName("id_empresa");
            Property(s => s.Nome).HasColumnName("nome");
            Property(s => s.Ativo).HasColumnName("ativo").IsOptional();
            Property(s => s.Obs).HasColumnName("obs").HasMaxLength(500).IsOptional();
        }
    }
}
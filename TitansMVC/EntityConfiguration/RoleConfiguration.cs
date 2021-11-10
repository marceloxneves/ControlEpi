using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using TitansMVC.Models;

namespace TitansMVC.EntityConfiguration
{
    public class RoleConfiguration : EntityTypeConfiguration<RoleModel>
    {
        public RoleConfiguration()
        {
            ToTable("AspNetRoles");

            HasKey(r => r.Id);
            Property(r => r.Id).HasColumnName("Id");
            Property(r => r.Nome).HasColumnName("Name");
            Property(r => r.Descricao).HasColumnName("Descricao");
            Property(r => r.Ativo).HasColumnName("Ativo");
        }
    }
}
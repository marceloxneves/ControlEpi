using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using TitansMVC.Models;

namespace TitansMVC.EntityConfiguration
{
    public class PermissaoUsuarioConfiguration : EntityTypeConfiguration<PermissaoUsuarioModel>
    {
        public PermissaoUsuarioConfiguration()
        {
            ToTable("AspNetUserRoles");

            Property(p => p.Id).HasColumnName("Id");
            Property(p => p.IdUsuario).HasColumnName("UserId");
            Property(p => p.IdPermissao).HasColumnName("RoleId");
            Property(p => p.DescricaoPermissao).HasColumnName("DescricaoRole");

            HasRequired(p => p.Usuario).WithMany().HasForeignKey(p => p.IdUsuario).WillCascadeOnDelete(true);
        }
    }
}
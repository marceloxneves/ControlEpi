using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using TitansMVC.Models;

namespace TitansMVC.EntityConfiguration
{
    public class UsuarioConfiguration : EntityTypeConfiguration<UsuarioModel>
    {
        public UsuarioConfiguration()
        {
            ToTable("AspNetUsers");
            HasKey(u => u.Id);

            Property(u => u.Id).HasColumnName("Id");
            Property(u => u.Email).HasColumnName("Email");
            Property(u => u.EmailConfirmed).HasColumnName("EmailConfirmed");
            Property(u => u.Username).HasColumnName("UserName");
            Property(u => u.Ativo).HasColumnName("Active");
            Property(u => u.Obs).HasColumnName("Obs").HasMaxLength(500).IsOptional();

            //Ignore(u => u.PasswordHash);
            //Ignore(u => u.SecurityStamp);
            //Ignore(u => u.PhoneNumber);
            //Ignore(u => u.PhoneNumberConfirmed);
            //Ignore(u => u.TwoFactorEnabled);
            //Ignore(u => u.AccessFailedCount);
            //Ignore(u => u.LockoutEnabled);
            //Ignore(u => u.LockoutEndDateUtc);

            HasRequired(u => u.Empresa).WithMany().HasForeignKey(u => u.IdEmpresa);
            HasMany<PermissaoUsuarioModel>(u => u.Permissoes).WithRequired(p => p.Usuario).HasForeignKey(p => p.IdUsuario).WillCascadeOnDelete(true);
        }
    }
}
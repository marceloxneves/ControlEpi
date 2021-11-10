using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using TitansMVC.Models;

namespace TitansMVC.EntityConfiguration
{
    public class EmailConfirmacaoRegistroConfig: EntityTypeConfiguration<EmailConfirmacaoRegistroModel>
    {
        public EmailConfirmacaoRegistroConfig()
        {
            ToTable("emails_confirm_registro");
            HasKey(e => e.Id);

            Property(e => e.Id).HasColumnName("id");
            Property(e => e.Email).HasColumnName("email");
        }
    }
}
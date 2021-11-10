using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using TitansMVC.Models;

namespace TitansMVC.EntityConfiguration
{
    public class PlanoConfiguration : EntityTypeConfiguration<PlanoModel>
    {
        public PlanoConfiguration()
        {
            ToTable("schema_control");
            HasKey(c => c.Id);

            Property(c => c.Id).HasColumnName("id");
            Property(c => c.CnpjCriptografado).HasColumnName("schema_reg");
            Property(c => c.NivelPlano).HasColumnName("schema_type");
            Property(c => c.ValidadeCriptografada).HasColumnName("schema_val");
            Property(c => c.DataUltimaValidacao).HasColumnName("schema_inf");

            Ignore(c => c.Validade);
            Ignore(c => c.Cnpj);
        }
    }
}
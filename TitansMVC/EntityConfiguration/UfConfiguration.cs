using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using TitansMVC.Models;

namespace TitansMVC.EntityConfiguration
{
    public class UfConfiguration : EntityTypeConfiguration<UfModel>
    {
        public UfConfiguration()
        {
            ToTable("uf");
            HasKey(u => u.Id);
            Property(u => u.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(u => u.Id).HasColumnName("id");
            Property(u => u.Sigla).HasMaxLength(2).HasColumnName("sigla");
            Property(u => u.Nome).HasColumnName("nome");
            Property(u => u.CodIbge).HasMaxLength(2).HasColumnName("cod_ibge");
            Property(u => u.Obs).HasMaxLength(500).HasColumnName("obs");
            Ignore(u => u.DataCad);
            //Property(u => u.DataCad).HasColumnName("DATA_CAD").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
        }
    }
}
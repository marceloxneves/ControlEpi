using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using TitansMVC.Models;

namespace TitansMVC.EntityConfiguration
{
    public class MunicipioConfiguration : EntityTypeConfiguration<MunicipioModel>
    {
        public MunicipioConfiguration()
        {
            ToTable("municipios");
            HasKey(m => m.Id);

            Property(m => m.Id).HasColumnName("id");
            Property(m => m.Nome).HasColumnName("nome").IsRequired();
            Property(m => m.CodIbge).HasColumnName("cod_ibge").HasMaxLength(7).IsRequired();
            Property(m => m.UfId).HasColumnName("uf_id").IsRequired();
            Property(m => m.Obs).HasColumnName("obs").HasMaxLength(500).IsOptional();
            Ignore(u => u.DataCad);

            HasRequired(m => m.Uf).WithMany().HasForeignKey(m => m.UfId);
        }
    }
}
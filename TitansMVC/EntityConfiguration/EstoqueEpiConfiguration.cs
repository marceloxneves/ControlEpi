using System;
using System.Data.Entity.ModelConfiguration;
using TitansMVC.Models;

namespace TitansMVC.EntityConfiguration
{
    public class EstoqueEpiConfiguration:EntityTypeConfiguration<EstoqueEpi>
    {
        public EstoqueEpiConfiguration()
        {
            ToTable("estoque_epi");
            HasKey(e => e.Id);

            Property(e => e.Id).HasColumnName("id");
            Property(e => e.IdEmpresa).HasColumnName("id_empresa");
            Property(e => e.IdEpi).HasColumnName("id_epi").IsRequired();
            Property(e => e.Qtde).HasColumnName("qtde").IsRequired();
            Property(e => e.QtdeMax).HasColumnName("qtde_max").IsOptional();
            Property(e => e.QtdeMin).HasColumnName("qtde_min").IsOptional();
            Property(e => e.DataCad).HasColumnName("data_cad").IsOptional();

            HasRequired(e => e.Epi).WithMany().HasForeignKey(e => e.IdEpi);
        }
    }
}

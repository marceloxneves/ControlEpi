using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using TitansMVC.Models;

namespace TitansMVC.EntityConfiguration
{
    public class SetorConfiguration : EntityTypeConfiguration<SetorModel>
    {
        public SetorConfiguration()
        {
            ToTable("setor");
            HasKey(s => s.Id);

            Property(s => s.Id).HasColumnName("id");
            Property(s => s.IdEmpresa).HasColumnName("id_empresa");
            Property(s => s.Nome).HasColumnName("nome");
            Property(s => s.Ativo).HasColumnName("ativo").IsOptional();
            Property(s => s.Importado).HasColumnName("importado").IsOptional();
            Property(s => s.Obs).HasColumnName("obs").HasMaxLength(500).IsOptional();
            Property(c => c.UnidadeNegocioId).HasColumnName("id_unidade_negocio");

            //Ignore(s => s.DataCad);
            //alterado para gravar data de cadastro
            Property(s => s.DataCad).HasColumnName("data_cad").IsOptional();

            HasRequired(s => s.Empresa).WithMany().HasForeignKey(s => s.IdEmpresa);
            HasMany<EpiSetorModel>(s => s.Epis).WithRequired(e => e.Setor).HasForeignKey(e => e.SetorId).WillCascadeOnDelete(true);
            HasRequired(c => c.UnidadeNegocio).WithMany().HasForeignKey(c => c.UnidadeNegocioId);
            HasMany<ColaboradorModel>(s => s.Colaboradores).WithOptional(c => c.Setor).HasForeignKey(c => c.SetorId).WillCascadeOnDelete(false);
            HasMany<UniformeSetorModel>(s => s.Uniformes).WithRequired(e => e.Setor).HasForeignKey(e => e.SetorId).WillCascadeOnDelete(true);
        }
    }
}
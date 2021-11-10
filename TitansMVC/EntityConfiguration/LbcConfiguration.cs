using System.Data.Entity;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using TitansMVC.Models;

namespace TitansMVC.EntityConfiguration
{
    public class LbcConfiguration: EntityTypeConfiguration<LbcModel>
    {
        public LbcConfiguration()
        {
            ToTable("lbc");
            HasKey(l => l.Id);

            Property(l => l.Id).HasColumnName("id");
            Property(l => l.IdEmpresa).HasColumnName("id_empresa");
            Property(l => l.Nome).HasColumnName("nome");
            Property(l => l.Ativo).HasColumnName("ativo").IsOptional();
            Property(l => l.Obs).HasColumnName("obs").HasMaxLength(500).IsOptional();
            Property(l => l.Fantasia).HasColumnName("fantasia").IsOptional();
            Property(l => l.Cnpj).HasColumnName("cnpj").HasMaxLength(20).IsOptional();
            Property(l => l.InscrEst).HasColumnName("inscr_est").HasMaxLength(20).IsOptional();
            Property(l => l.InscrMun).HasColumnName("inscr_mun").HasMaxLength(20).IsOptional();
            Property(l => l.Url).HasColumnName("url").IsOptional();
            Property(l => l.EndEndereco).HasColumnName("end_endereco").IsOptional();
            Property(l => l.EndNumero).HasColumnName("end_numero").HasMaxLength(20).IsOptional();
            Property(l => l.EndComplemento).HasColumnName("end_complemento").HasMaxLength(50).IsOptional();
            Property(l => l.EndBairro).HasColumnName("end_bairro").IsOptional();
            Property(l => l.EndCep).HasColumnName("end_cep").HasMaxLength(20).IsOptional();
            Property(l => l.MunicipioId).HasColumnName("id_municipio").IsOptional();
            Property(l => l.Obs).HasColumnName("obs").HasMaxLength(500).IsOptional();
            Property(l => l.SiglaUf).HasColumnName("sigla_uf").HasMaxLength(2).IsOptional();
            Property(l => l.ProxNumOs).HasColumnName("prox_num_os").IsOptional();
            Property(l => l.Ativo).HasColumnName("ativo");

            //Ignore(l => l.DataCad);
            Property(l => l.DataCad).HasColumnName("data_cad").IsOptional();

            HasRequired(l => l.Empresa).WithMany().HasForeignKey(l => l.IdEmpresa);
            HasMany<CentroCustoModel>(l => l.CentrosDeCustos).WithOptional(c => c.Lbc).HasForeignKey(c => c.LbcId).WillCascadeOnDelete(false);
        }
    }
}
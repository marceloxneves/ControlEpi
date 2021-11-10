using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using TitansMVC.Models;

namespace TitansMVC.EntityConfiguration
{
    public class EmpresaConfiguration : EntityTypeConfiguration<EmpresaModel>
    {
        public EmpresaConfiguration()
        {
            ToTable("empresa");
            HasKey(e => e.Id);
            Property(e => e.Id).HasColumnName("id");
            Property(e => e.Razao).HasColumnName("razao").IsRequired();
            Property(e => e.Fantasia).HasColumnName("fantasia").IsOptional();
            Property(e => e.Cnpj).HasColumnName("cnpj").HasMaxLength(20).IsOptional();
            Property(e => e.InscrEst).HasColumnName("inscr_est").HasMaxLength(20).IsOptional();
            Property(e => e.InscrMun).HasColumnName("inscr_mun").HasMaxLength(20).IsOptional();
            Property(e => e.Url).HasColumnName("url").IsOptional();
            //Property(e => e.EndLogradouro).HasColumnName("end_logradouro").HasMaxLength(20).IsOptional();
            Property(e => e.EndEndereco).HasColumnName("end_endereco").IsOptional();
            Property(e => e.EndNumero).HasColumnName("end_numero").HasMaxLength(20).IsOptional();
            Property(e => e.EndComplemento).HasColumnName("end_complemento").HasMaxLength(50).IsOptional();
            Property(e => e.EndBairro).HasColumnName("end_bairro").IsOptional();
            Property(e => e.EndCep).HasColumnName("end_cep").HasMaxLength(20).IsOptional();
            Property(e => e.MunicipioId).HasColumnName("id_municipio").IsOptional();
            Property(e => e.Obs).HasColumnName("obs").HasMaxLength(500).IsOptional();
            Property(e => e.SiglaUf).HasColumnName("sigla_uf").HasMaxLength(2).IsOptional();
            Property(e => e.ProxNumOs).HasColumnName("prox_num_os").IsOptional();
            Property(e => e.Matriz).HasColumnName("matriz").IsOptional();
            Property(e => e.Ativo).HasColumnName("ativo");
            Property(e => e.PossuiBiometria).HasColumnName("possui_biometria");
            Property(e => e.PossuiAssinatura).HasColumnName("possui_assinatura");
            Property(e => e.Logomarca).HasColumnName("logomarca").IsOptional();

            //Ignore(e => e.DataCad);

            HasOptional(e => e.Municipio).WithMany().HasForeignKey(e => e.MunicipioId);
            HasMany<TelefoneEmpresaModel>(e => e.Telefones).WithRequired(t => t.Empresa).HasForeignKey(t => t.IdEmpresa);
            HasMany<EmailEmpresaModel>(e => e.Emails).WithRequired(t => t.Empresa).HasForeignKey(t => t.IdEmpresa);

            //HasOptional(e => e.Telefones).WithOptionalPrincipal().WillCascadeOnDelete(true);
        }
    }
}
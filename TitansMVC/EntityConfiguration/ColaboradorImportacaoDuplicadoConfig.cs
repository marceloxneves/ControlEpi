using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using TitansMVC.Models;

namespace TitansMVC.EntityConfiguration
{
    public class ColaboradorImportacaoDuplicadoConfig : EntityTypeConfiguration<ColaboradorImportacaoDuplicadoModel>
    {
        public ColaboradorImportacaoDuplicadoConfig()
        {
            ToTable("colaboradores_importacao_duplicados");
            HasKey(c => c.Id);

            Property(c => c.Id).HasColumnName("id");
            Property(c => c.Nome).HasColumnName("nome");
            Property(c => c.Cpf).HasColumnName("cpf");
            Property(c => c.NumRegEmpresa).HasColumnName("num_reg_empresa");
            Property(c => c.Genero).HasColumnName("genero");
            Property(c => c.DataNascimento).HasColumnName("data_nascimento");
            Property(c => c.DataAdmissao).HasColumnName("data_admissao");
            Property(c => c.RecebeuTreinamento).HasColumnName("recebeu_treinamento");
            Property(c => c.RecebeuAdvertencia).HasColumnName("recebeu_advertencia");
            Property(c => c.MotivoAdvertencia).HasColumnName("motivo_advertencia");
            Property(c => c.Obs).HasColumnName("obs");
            Property(c => c.Setor).HasColumnName("setor");
            Property(c => c.DataHora).HasColumnName("data_hora");
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using TitansMVC.Models;

namespace TitansMVC.EntityConfiguration
{
    public class EpiColaboradorConfiguration: EntityTypeConfiguration<EpiColaboradorModel>
    {
        public EpiColaboradorConfiguration()
        {
            ToTable("colaborador_epi");
            HasKey(e => e.Id);

            Property(e => e.Id).HasColumnName("id");
            Property(e => e.IdEmpresa).HasColumnName("id_empresa");
            Property(e => e.TipoEpiId).HasColumnName("id_tipo_epi");
            Property(e => e.NomeEpi).HasColumnName("nome_epi");
            Property(e => e.EpiSetorId).HasColumnName("id_epi_setor");
            Property(e => e.ColaboradorId).HasColumnName("id_colaborador");
            Property(e => e.DataEntrega).HasColumnName("data_entrega").IsOptional();
            Property(e => e.Quantidade).HasColumnName("qtde").IsOptional();
            Property(e => e.NomeSetor).HasColumnName("nome_setor").IsOptional();
            Property(c => c.CentroCustoId).HasColumnName("id_centro_custo").IsOptional();
            Property(e => e.ValidadeEmDias).HasColumnName("validade_dias").IsOptional();
            Property(e => e.AssinaturaColaborador).HasColumnName("assinatura_colaborador").IsOptional();
            Property(e => e.DataVencimento).HasColumnName("data_vencimento").IsOptional();
            Property(e => e.DataHoraBaixa).HasColumnName("data_hora_baixa").IsOptional();
            Property(e => e.Baixado).HasColumnName("baixado");
            Property(e => e.TipoValidacao).HasColumnName("tipo_validacao");
            Property(e => e.AssinaturaPendente).HasColumnName("assin_pendente");
            Property(e => e.Justificativa).HasColumnName("justificativa_baixa").IsOptional();
            Property(e => e.JustificativaEpiOutroSetor).HasColumnName("justificativa").IsOptional();
            Property(e => e.UnidadeNegocioId).HasColumnName("id_unidade_negocio");

            //HasRequired(e => e.EpiSetor).WithMany().HasForeignKey(e => e.EpiSetorId);
            HasRequired(e => e.Empresa).WithMany().HasForeignKey(e => e.IdEmpresa);
            HasOptional(e => e.CentroCusto).WithMany().HasForeignKey(e => e.CentroCustoId);
            HasRequired(e => e.Colaborador).WithMany().HasForeignKey(e => e.ColaboradorId);
            HasRequired(e => e.UnidadeNogocio).WithMany().HasForeignKey(e => e.UnidadeNegocioId);
        }
    }
}
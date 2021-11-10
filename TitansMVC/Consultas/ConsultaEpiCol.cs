using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using TitansMVC.Models.Enums;
using TitansMVC.Models.Relatorios;
using TitansMVC.Utils;

namespace TitansMVC.Consultas
{
    public class ConsultaEpiCol
    {
        public static string getConsulta(ColaboradorEpiFilter filtro)
        {
            StringBuilder consulta = new StringBuilder();

            consulta.Append("select c.id as id_colaborador, c.nome as nome_colaborador, ec.nome_epi, ");
            consulta.Append("ec.id as id_colaborador_epi, ec.qtde as qtde_epis, ");
            consulta.Append("ec.data_entrega, ec.validade_dias, ec.data_vencimento, ");
            consulta.Append("ec.assinatura_colaborador, ec.baixado, ec.data_hora_baixa, ec.tipo_validacao, ");            
            consulta.Append("e.id as id_epi, e.ca as epi_ca, cc.id as id_centro_custo, ");
            consulta.Append("cc.nome as nome_ccusto, s.nome as nome_setor, f.nome as funcao, ");
            consulta.Append("ec.id_unidade_negocio as id_unidade_negocio, un.fantasia as nome_fantasia, un.nome as razao_social, ");
            consulta.Append("un.logomarca as logomarca_lbc, ");
            consulta.Append("case  when ec.tipo_validacao = 0 then 'NÃO' else 'SIM' end possui_biometria ");
            consulta.Append("from [controlepi_hard].[colaborador] c ");
            consulta.Append("right join [controlepi_hard].[colaborador_epi] ec on(c.id = ec.id_colaborador and c.id_empresa = ec.id_empresa) ");
            consulta.Append("left join [controlepi_hard].[setor_epi] es on(ec.id_epi_setor = es.id) ");
            consulta.Append("left join [controlepi_hard].[epi] e on(es.id_epi = e.id) ");
            consulta.Append("left join [controlepi_hard].[setor] s on(c.id_setor = s.id) ");
            consulta.Append("left join [controlepi_hard].[funcao] f on(c.id_funcao = f.id) ");
            consulta.Append("left join [controlepi_hard].[centro_custo] cc on(ec.id_centro_custo = cc.id) ");
            consulta.Append("left join [controlepi_hard].[lbc] un on(ec.id_unidade_negocio = un.id and ec.id_empresa = un.id_empresa) ");
            consulta.Append(string.Format("where (c.ativo = {0}) and (c.id_empresa = {1}) ", filtro.Ativos ? 1 : 0, Util.GetEmpresaId()));

            if (filtro.UnidadeNegocioId != 0 && !String.IsNullOrWhiteSpace(filtro.UnidadeNegocioId.ToString()))
            {
                //consulta.Append("and (c.id_unidade_negocio = " + filtro.UnidadeNegocioId + ") ");
                consulta.Append("and (ec.id_unidade_negocio = " + filtro.UnidadeNegocioId + ") ");
            }

            if (!String.IsNullOrWhiteSpace(filtro.ColaboradorId.ToString()) && filtro.ColaboradorId != 0)
            {
                consulta.Append("and (c.id = " + filtro.ColaboradorId + ") ");
            }

            if (!String.IsNullOrWhiteSpace(filtro.EpiId.ToString()) && filtro.EpiId != 0)
            {
                consulta.Append("and (e.id = " + filtro.EpiId + ") ");
            }

            if (!String.IsNullOrWhiteSpace(filtro.TipoEpiId.ToString()) && filtro.TipoEpiId != 0)
            {
                consulta.Append("and (ec.id_tipo_epi = " + filtro.TipoEpiId + ") ");
            }

            if (!String.IsNullOrWhiteSpace(filtro.Marca))
            {
                consulta.Append("and (e.marca = '" + filtro.Marca + "') ");
            }

            if (!String.IsNullOrWhiteSpace(filtro.SetorId.ToString()) && filtro.SetorId != 0)
            {
                consulta.Append("and (s.id = " + filtro.SetorId + ") ");
            }

            if (!String.IsNullOrWhiteSpace(filtro.CentroCustoId.ToString()) && filtro.CentroCustoId != 0)
            {
                consulta.Append("and (cc.id = " + filtro.CentroCustoId + ") ");
            }

            if ((filtro.DataInicial != null) && (filtro.DataFinal != null))
            {
                consulta.Append(String.Format("and (Convert(date, ec.data_entrega) between Convert(date, '{0}') and Convert(date, '{1}')) ", filtro.DataInicial.Value.ToString("yyyy-MM-dd 00:00:00"),
                    filtro.DataFinal.Value.ToString("yyyy-MM-dd 00:00:00")));
            }

            switch (filtro.EstadoEpis)
            {
                case EstadoEpisConsulta.Entregues:
                {
                    consulta.Append("and (ec.baixado = 0 or ec.baixado is null) ");
                    break;
                }

                case EstadoEpisConsulta.Baixados:
                {
                    consulta.Append("and (ec.baixado = 1) ");
                    break;
                }                
            }

            consulta.Append("order by c.nome, ec.nome_epi, ec.data_entrega");

            return consulta.ToString();
        }
    }
}
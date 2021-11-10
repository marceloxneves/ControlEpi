using System;
using System.Text;
using TitansMVC.Models.Relatorios;
using TitansMVC.Utils;

namespace TitansMVC.Consultas
{
    public class ConsultaConsumoUniforme
    {
        public static string GetConsulta(ConsumoUniformeFilter filtro)
        {
            StringBuilder consulta = new StringBuilder();

            consulta.Append("select c.id as id_colaborador, c.nome as nome_colaborador, ec.nome_uniforme, ");
            consulta.Append("ec.id as id_colaborador_uniforme, ec.qtde as qtde_uniformes, ");
            consulta.Append("ec.baixado, ");
            consulta.Append("e.id as id_uniforme, ISNULL(e.custo,0) as uniforme_custo, cc.id as id_centro_custo, ");
            consulta.Append("cc.nome as nome_ccusto,  ");

            //mod
            consulta.Append("ec.nome_setor, cf.nome as nome_funcao, ");

            consulta.Append("ec.id_unidade_negocio as id_unidade_negocio, un.fantasia as nome_fantasia, un.nome as razao_social, ");

            consulta.Append("ec.justificativa, ec.justificativa_baixa ");

            consulta.Append("from [controlepi_hard].[colaborador] c ");
            consulta.Append("right join [controlepi_hard].[colaborador_uniforme] ec on(c.id = ec.id_colaborador and c.id_empresa = ec.id_empresa) ");
            consulta.Append("left join [controlepi_hard].[setor_uniforme] es on(ec.id_uniforme_setor = es.id) ");
            consulta.Append("left join [controlepi_hard].[uniforme] e on(es.id_uniforme = e.id) ");
            consulta.Append("left join [controlepi_hard].[setor] s on(es.id_setor = s.id) ");
            consulta.Append("left join [controlepi_hard].[centro_custo] cc on(ec.id_centro_custo = cc.id) ");
            
            //mod
            consulta.Append("left join [controlepi_hard].[funcao] cf on (c.id_funcao=cf.id) ");

            consulta.Append("left join [controlepi_hard].[lbc] un on(ec.id_unidade_negocio = un.id and ec.id_empresa = un.id_empresa) ");

            //trazer todos os funcionarios
            consulta.Append(string.Format("where (c.id_empresa = {0}) ", Util.GetEmpresaId()));

            if (!String.IsNullOrWhiteSpace(filtro.UnidadeNegocioId.ToString()) && filtro.UnidadeNegocioId != 0)
            {
                consulta.Append("and (ec.id_unidade_negocio = " + filtro.UnidadeNegocioId + ") ");
            }

            if (!String.IsNullOrWhiteSpace(filtro.ColaboradorId.ToString()) && filtro.ColaboradorId != 0)
            {
                consulta.Append("and (c.id = " + filtro.ColaboradorId + ") ");
            }

            if (!String.IsNullOrWhiteSpace(filtro.UniformeId.ToString()) && filtro.UniformeId != 0)
            {
                consulta.Append("and (e.id = " + filtro.UniformeId + ") ");
            }

            if (!String.IsNullOrWhiteSpace(filtro.TipoUniformeId.ToString()) && filtro.TipoUniformeId != 0)
            {
                consulta.Append("and (ec.id_tipo_uniforme = " + filtro.TipoUniformeId + ") ");
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

            consulta.Append("order by ec.nome_uniforme");

            return consulta.ToString();
        }
    }
}
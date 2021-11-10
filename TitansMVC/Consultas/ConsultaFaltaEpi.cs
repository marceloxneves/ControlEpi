using System;
using System.Text;
using TitansMVC.Models.Relatorios;
using TitansMVC.Utils;

namespace TitansMVC.Consultas
{
    public class ConsultaFaltaEpi
    {
        public static string GetConsulta(RelEpiFaltaFilter filtro)
        {
            StringBuilder consulta = new StringBuilder();

            consulta.Append("select ep.id as id, ep.nome as nome_epi, ep.marca, ep.ca, ");
            consulta.Append("ep.cod_epi_titans as cod_interno, es.qtde as qtde_estoque, es.qtde_min ");
            consulta.Append("from[controlepi_hard].[epi] ep ");
            consulta.Append("left join[controlepi_hard].[estoque_epi] es on ep.id = es.id_epi ");
            consulta.Append(string.Format("where (ep.ativo = 1) and (es.qtde <= es.qtde_min) and (ep.id_empresa = {0}) ", Util.GetEmpresaId()));

            if (!String.IsNullOrWhiteSpace(filtro.EpiId.ToString()) && filtro.EpiId != 0)
            {
                consulta.Append("and (ep.id = " + filtro.EpiId + ") ");
            }

            if (!String.IsNullOrWhiteSpace(filtro.DescrEpi))
            {
                consulta.Append("and (ep.nome like '%" + filtro.DescrEpi + "%') ");
            }

            if (!String.IsNullOrWhiteSpace(filtro.Marca))
            {
                consulta.Append("and (ep.marca like '%" + filtro.Marca + "%') ");
            }


            consulta.Append("order by ep.nome");

            return consulta.ToString();
        }
    }
}

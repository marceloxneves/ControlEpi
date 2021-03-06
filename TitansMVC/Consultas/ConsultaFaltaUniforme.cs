using System;
using System.Text;
using TitansMVC.Models.Relatorios;
using TitansMVC.Utils;

namespace TitansMVC.Consultas
{
    public class ConsultaFaltaUniforme
    {
        public static string GetConsulta(RelUniformeFaltaFilter filtro)
        {
            StringBuilder consulta = new StringBuilder();

            consulta.Append("select ep.id as id, ep.nome as nome_uniforme, ep.marca, ");
            consulta.Append("ep.cod_uniforme_titans as cod_interno, es.qtde as qtde_estoque, es.qtde_min ");
            consulta.Append("from[controlepi_hard].[uniforme] ep ");
            consulta.Append("left join[controlepi_hard].[estoque_uniforme] es on ep.id = es.id_uniforme ");
            consulta.Append(string.Format("where (ep.ativo = 1) and (es.qtde <= es.qtde_min) and (ep.id_empresa = {0}) ", Util.GetEmpresaId()));

            if (!String.IsNullOrWhiteSpace(filtro.UniformeId.ToString()) && filtro.UniformeId != 0)
            {
                consulta.Append("and (ep.id = " + filtro.UniformeId + ") ");
            }

            if (!String.IsNullOrWhiteSpace(filtro.DescrUniforme))
            {
                consulta.Append("and (ep.nome like '%" + filtro.DescrUniforme + "%') ");
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

using System;
using System.Text;

namespace TitansMVC.Consultas
{
    public class ConsultaFichaUniforme
    {
        public static string GetConsulta(int idUniforme)
        {
            StringBuilder consulta = new StringBuilder();

            consulta.Append("SELECT f.id as id_ficha, ");
            consulta.Append("f.id_uniforme, ");
            consulta.Append("f.uniforme_nome, ");
            consulta.Append("f.revisado_em, ");
            consulta.Append("f.descricao_det_equip, ");
            consulta.Append("f.fornecedor_fabricante, ");
            consulta.Append("f.forma_higienizacao, ");
            consulta.Append("f.finalidade_area_aplic, ");
            consulta.Append("f.obs, ");
            consulta.Append("f.aprovado_por, ");
            consulta.Append("f.registro, ");
            consulta.Append("f.area, ");
            consulta.Append("f.pecas_reposicao, ");
            consulta.Append("f.num_identificacao, ");
            consulta.Append("e.foto ");
            consulta.Append("FROM [controlepi_hard].[ficha_uniforme] f ");
            consulta.Append("LEFT JOIN [controlepi_hard].[uniforme] e ");
            consulta.Append("ON (f.id_uniforme = e.id) ");
            consulta.Append(String.Format("WHERE (e.id = {0})", idUniforme));

            return consulta.ToString();
        }
    }
}
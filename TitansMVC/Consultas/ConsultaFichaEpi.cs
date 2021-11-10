using System;
using System.Text;

namespace TitansMVC.Consultas
{
    public class ConsultaFichaEpi
    {
        public static string GetConsulta(int idEpi)
        {
            StringBuilder consulta = new StringBuilder();

            consulta.Append("SELECT f.id as id_ficha, ");
            consulta.Append("f.id_epi, ");
            consulta.Append("f.epi_nome, ");
            consulta.Append("f.epi_ca, ");
            consulta.Append("f.epi_validade_ca, ");
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
            consulta.Append("FROM [controlepi_hard].[ficha_epi] f ");
            consulta.Append("LEFT JOIN [controlepi_hard].[epi] e ");
            consulta.Append("ON (f.id_epi = e.id) ");
            consulta.Append(String.Format("WHERE (e.id = {0})", idEpi));

            return consulta.ToString();
        }
    }
}
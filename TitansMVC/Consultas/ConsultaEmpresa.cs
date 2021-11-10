using System.Text;
using TitansMVC.Utils;

namespace TitansMVC.Consultas
{
    public class ConsultaEmpresa
    {
        public static string GetConsulta()
        {
            StringBuilder consulta = new StringBuilder();

            consulta.Append("SELECT TOP 1 e.id ");
            consulta.Append(" ,e.razao as razao_social");
            consulta.Append(" ,e.fantasia");
            consulta.Append(" ,e.cnpj");
            consulta.Append(" ,e.inscr_est");
            consulta.Append(" ,e.inscr_mun");
            consulta.Append(" ,e.cnae");
            consulta.Append(" ,e.url");
            consulta.Append(" ,case when (e.end_logradouro <> '') then");
            consulta.Append(" e.end_logradouro + ' ' + e.end_endereco");
            consulta.Append(" else e.end_endereco end ");
            consulta.Append(" + case when (e.end_numero <> '')");
            consulta.Append(" then ', ' + e.end_numero else '' end +");
            consulta.Append(" case when ( e.end_complemento <> '') then ', '");
            consulta.Append(" + e.end_complemento else '' end +");
            consulta.Append(" case when (e.end_bairro <> '') then ', '");
            consulta.Append(" + e.end_bairro else '.' end +");
            consulta.Append(" case when (m.nome <> '') then ', '");
            consulta.Append(" + m.nome else '.' end +");
            consulta.Append(" case when (e.sigla_uf <> '') then ' - '");
            consulta.Append(" + e.sigla_uf + ', ' else '' end +");
            consulta.Append(" case when (e.end_cep <> '') then '  Cep: '");
            consulta.Append(" + e.end_cep + '.' else '.' end as endereco");
            consulta.Append(" ,e.prox_num_os");
            consulta.Append(" ,e.matriz");
            consulta.Append(" ,e.obs");
            consulta.Append(" ,e.logomarca");
            consulta.Append(" ,(select top 1 email from [controlepi_hard].[empresa_email]");
            consulta.Append(" where id_empresa = e.id) as email");
            consulta.Append(" ,(select top 1 tel_numero from [controlepi_hard].[empresa_telefone]");
            consulta.Append(" where id_empresa = e.id) as telefone from [controlepi_hard].[empresa] e");
            consulta.Append(" left join [controlepi_hard].[municipios] m on");
            consulta.Append(" (m.id = e.id_municipio)");
            consulta.Append(string.Format(" where (e.ativo = 1) and e.id = {0}", Util.GetEmpresaId()));

            return consulta.ToString();
        }
    }
}
using System.Linq;
using TitansMVC.Models;
using TitansMVC.Repository.Interfaces;
using TitansMVC.Utils;

namespace TitansMVC.Repository.Implementations
{
    public class ConfiguracaoRepository : RepositoryBase<ConfiguracaoModel>, IConfiguracaoRepository
    {
        public ConfiguracaoModel GetUnique()
        {
            int idEmpresa = Util.GetEmpresaId();

            return Db.Configuracoes.FirstOrDefault(c => c.IdEmpresa == idEmpresa);
        }
    }
}
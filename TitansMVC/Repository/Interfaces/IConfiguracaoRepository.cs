using TitansMVC.Models;

namespace TitansMVC.Repository.Interfaces
{
    public interface IConfiguracaoRepository : IRepositoryBase<ConfiguracaoModel>
    {
        ConfiguracaoModel GetUnique();
    }
}

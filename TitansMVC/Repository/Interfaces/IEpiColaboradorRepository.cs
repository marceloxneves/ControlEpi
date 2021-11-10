using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TitansMVC.Models;

namespace TitansMVC.Repository.Interfaces
{
    public interface IEpiColaboradorRepository: IRepositoryBase<EpiColaboradorModel>
    {
        EpiColaboradorModel GetByIdEager(int idEpi);
        IEnumerable<EpiColaboradorModel> BuscarEpisPorColaborador(int idColaborador);
        IEnumerable<EpiColaboradorModel> BuscarNaoBaixados(int idColaborador);
        IEnumerable<EpiColaboradorModel> BuscarBaixados(int idColaborador);
        IEnumerable<EpiColaboradorModel> BuscarEpisVencidos(int idColaborador);
        IEnumerable<EpiColaboradorModel> BuscarEpisVencidos();
        IEnumerable<EpiColaboradorModel> BuscarEpisAVencer(int dias);
        bool EstaEmColaborador(int idEpiSetor);
    }
}

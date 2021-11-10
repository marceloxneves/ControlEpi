using System;
using System.Collections.Generic;
using TitansMVC.Models;

namespace TitansMVC.Repository.Interfaces
{
    public interface IEpiRepository: IRepositoryBase<EpiModel>
    {
        EpiModel GetByIdEager(int id);
        IEnumerable<EpiModel> GetAll(bool ativos);
        IEnumerable<EpiModel> BuscarInativos();
        IEnumerable<EpiModel> BuscarPorId(int id);
        IEnumerable<EpiModel> BuscarPorTipo(int idTipo); 
        IEnumerable<EpiModel> BuscarPorNome(string nome,bool ativos);
        IEnumerable<EpiModel> BuscarAtivos();
        IEnumerable<EpiModel> BuscarCaVencidos();
        IEnumerable<EpiModel> BuscarCaAVencer(int dias);
        EpiModel GetByCa(string ca);
        EpiModel GetByNomeAndMarca(string nome, string marca);
        bool EstaEmSetor(int idEpi);
        EpiModel BuscarPorCodigoDeBarras(string codigoDeBarras);
        EpiModel ValidarCodigoDeBarras(string codigoDeBarras, int idEpi);
    }
}

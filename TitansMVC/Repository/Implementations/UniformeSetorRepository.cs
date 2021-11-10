using System.Collections.Generic;
using System.Linq;
using TitansMVC.Models;
using TitansMVC.Repository.Interfaces;

namespace TitansMVC.Repository.Implementations
{
    public class UniformeSetorRepository : RepositoryBase<UniformeSetorModel>, IUniformeSetorRepository
    {
        public IEnumerable<UniformeSetorModel> BuscarPorTipo(int idTipo)
        {
            return Db.UniformesSetores.Where(e => e.TipoUniformeId == idTipo);
        }

        public IEnumerable<UniformeSetorModel> BuscarPorTipoESetor(int idTipo, int idSetor)
        {
            return Db.UniformesSetores.Where(e=>e.Uniforme.Ativo).Where(e => e.TipoUniformeId == idTipo).Where(e => e.SetorId == idSetor).ToList();
        }

        public IEnumerable<UniformeSetorModel> BuscarPorTipoEOutrosSetores(int idTipo, int idSetor)
        {
            return Db.UniformesSetores.Where(e => e.Uniforme.Ativo).Where(e => e.TipoUniformeId == idTipo).Where(e => e.SetorId != idSetor).OrderBy(e => e.NomeUniforme).ToList();
        }

        public IEnumerable<UniformeSetorModel> BuscarPorSetor(int idSetor)
        {
            return Db.UniformesSetores.Where(e => e.SetorId == idSetor).OrderBy(e => e.TipoUniforme);
        }

        public IEnumerable<UniformeSetorModel> BuscarPorOutrosSetores(int idSetor)
        {
            return Db.UniformesSetores.Where(e => e.SetorId != idSetor).OrderBy(e => e.TipoUniforme);
        }

        public IEnumerable<UniformeSetorModel> BuscarPorUniforme(int idUniforme)
        {
            return Db.UniformesSetores.Where(e => e.UniformeId == idUniforme).ToList();
        }
        
        public bool UniformeSetorEntregue(int idUniformeSetor)
        {
            return Db.UniformesColaboradores.Where(e => e.UniformeSetorId == idUniformeSetor).Any(e => !e.Baixado.Value);
        }
    }
}
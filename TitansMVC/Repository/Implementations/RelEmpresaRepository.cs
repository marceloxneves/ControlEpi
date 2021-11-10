using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using TitansMVC.Context;
using TitansMVC.Models.Relatorios;
using TitansMVC.Repository.Interfaces;

namespace TitansMVC.Repository.Implementations
{
    public class RelEmpresaRepository : RepositoryBase<RelEmpresaModel>, IRelEmpresaRepository
    {

    }
}
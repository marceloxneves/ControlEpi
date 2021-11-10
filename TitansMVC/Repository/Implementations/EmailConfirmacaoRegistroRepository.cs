using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TitansMVC.Models;
using TitansMVC.Repository.Interfaces;

namespace TitansMVC.Repository.Implementations
{
    public class EmailConfirmacaoRegistroRepository : RepositoryBase<EmailConfirmacaoRegistroModel>, IEmailConfirmacaoRegistroRepository
    {
    }
}
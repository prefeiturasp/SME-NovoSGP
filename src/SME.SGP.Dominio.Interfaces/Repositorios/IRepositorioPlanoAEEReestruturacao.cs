using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioPlanoAEEReestruturacao : IRepositorioBase<PlanoAEEReestruturacao>
    {
        Task<bool> ExisteReestruturacaoParaVersao(long versaoId, long reestruturacaoId);
    }
}

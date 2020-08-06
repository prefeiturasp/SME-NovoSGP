using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioDiarioBordo: IRepositorioBase<DiarioBordo>
    {
        Task<DiarioBordo> ObterPorAulaId(long aulaId);
        Task<bool> ExisteDiarioParaAula(long aulaId);
        Task ExcluirDiarioBordoDaAula(long aulaId);
    }
}

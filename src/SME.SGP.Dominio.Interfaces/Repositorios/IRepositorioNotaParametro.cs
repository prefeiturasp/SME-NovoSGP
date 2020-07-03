using System;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioNotaParametro : IRepositorioBase<NotaParametro>
    {
       Task<NotaParametro> ObterPorDataAvaliacao(DateTime dataAvaliacao);
    }
}
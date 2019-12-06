using System;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioNotaParametro : IRepositorioBase<NotaParametro>
    {
        NotaParametro ObterPorDataAvaliacao(DateTime dataAvaliacao);
    }
}
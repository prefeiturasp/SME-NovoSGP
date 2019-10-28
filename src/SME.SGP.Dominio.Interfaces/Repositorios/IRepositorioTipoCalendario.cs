using System.Collections.Generic;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioTipoCalendario : IRepositorioBase<TipoCalendario>
    {
        IEnumerable<TipoCalendario> ObterTiposCalendario();

        bool VerificarRegistroExistente(long id, string nome);

        TipoCalendario BuscarPorAnoLetivoEModalidade(int anoLetivo, Modalidade modalidade);
    }
}
using System.Collections.Generic;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioTipoCalendario : IRepositorioBase<TipoCalendario>
    {
        TipoCalendario BuscarPorAnoLetivoEModalidade(int anoLetivo, ModalidadeTipoCalendario modalidade);

        IEnumerable<TipoCalendario> ObterTiposCalendario();

        bool VerificarRegistroExistente(long id, string nome);
    }
}
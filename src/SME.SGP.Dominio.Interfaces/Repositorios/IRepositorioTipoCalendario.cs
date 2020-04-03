using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioTipoCalendario : IRepositorioBase<TipoCalendario>
    {
        TipoCalendario BuscarPorAnoLetivoEModalidade(int anoLetivo, ModalidadeTipoCalendario modalidade, int semestre = 0);

        IEnumerable<TipoCalendario> ListarPorAnoLetivo(int anoLetivo);

        IEnumerable<TipoCalendario> BuscarPorAnoLetivo(int anoLetivo);

        IEnumerable<TipoCalendario> ObterTiposCalendario();

        Task<bool> VerificarRegistroExistente(long id, string nome);
        Task<bool> PeriodoEmAberto(long tipoCalendarioId, DateTime dataReferencia, int bimestre = 0);
    }
}
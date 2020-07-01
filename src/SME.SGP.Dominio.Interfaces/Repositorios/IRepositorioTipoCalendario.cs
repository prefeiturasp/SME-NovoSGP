using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioTipoCalendario : IRepositorioBase<TipoCalendario>
    {
        Task<PeriodoEscolar> ObterPeriodoEscolarPorCalendarioEData(long tipoCalendarioId, DateTime dataParaVerificar);

        Task<TipoCalendario> BuscarPorAnoLetivoEModalidade(int anoLetivo, ModalidadeTipoCalendario modalidade, int semestre = 0);

        Task<long> ObterIdPorAnoLetivoEModalidadeAsync(int anoLetivo, ModalidadeTipoCalendario modalidade, int semestre = 0);

        Task<IEnumerable<TipoCalendario>> ListarPorAnoLetivo(int anoLetivo);

        Task<IEnumerable<TipoCalendario>> BuscarPorAnoLetivo(int anoLetivo);

        Task<IEnumerable<TipoCalendario>> ObterTiposCalendario();

        Task<bool> VerificarRegistroExistente(long id, string nome);

        Task<bool> PeriodoEmAberto(long tipoCalendarioId, DateTime dataReferencia, int bimestre = 0, bool ehAnoLetivo = false);

        Task<IEnumerable<TipoCalendario>> BuscarPorAnoLetivoEModalidade(int anoLetivo, ModalidadeTipoCalendario modalidade, DateTime dataReferencia);
    }
}
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioTipoCalendarioConsulta : IRepositorioBase<TipoCalendario>
    {
        Task<PeriodoEscolar> ObterPeriodoEscolarPorCalendarioEData(long tipoCalendarioId, DateTime dataParaVerificar);

        Task<TipoCalendario> BuscarPorAnoLetivoEModalidade(int anoLetivo, ModalidadeTipoCalendario modalidade, int? semestre = null);

        Task<long> ObterIdPorAnoLetivoEModalidadeAsync(int anoLetivo, ModalidadeTipoCalendario modalidade, int? semestre);

        Task<IEnumerable<TipoCalendario>> ListarPorAnoLetivoEModalidades(int anoLetivo, int[] modalidades, int? semestre = null);
        Task<IEnumerable<TipoCalendarioRetornoDto>> ListarPorAnoLetivoDescricaoEModalidades(int anoLetivo, string descricao, IEnumerable<int> modalidades);

        Task<IEnumerable<TipoCalendarioBuscaDto>> ListarPorAnosLetivoEModalidades(int[] anosLetivo, int[] modalidades, string nome);

        Task<string> ObterNomePorId(long tipoCalendarioId);

        Task<IEnumerable<TipoCalendario>> ObterTiposCalendario();

        Task<bool> VerificarRegistroExistente(long id, string nome);

        Task<bool> PeriodoEmAberto(long tipoCalendarioId, DateTime dataReferencia, int bimestre = 0, bool ehAnoLetivo = false);

        Task<IEnumerable<TipoCalendarioBuscaDto>> ObterTiposCalendarioPorDescricaoAsync(string descricao);
        Task<IEnumerable<PeriodoCalendarioBimestrePorAnoLetivoModalidadeDto>> ObterPeriodoTipoCalendarioBimestreAsync(int anoLetivo, ModalidadeTipoCalendario modalidadeTipoCalendario, int? semestre = null);
        Task<long> ObterTipoCalendarioIdPorAnoLetivoModalidadeEDataReferencia(int anoLetivo, ModalidadeTipoCalendario modalidadeTipoCalendarioId, DateTime dataReferencia);
        Task<int> ObterAnoLetivoUltimoTipoCalendarioPorDataReferencia(int anoReferencia, ModalidadeTipoCalendario modalidadeTipoCalendario);
    }
}
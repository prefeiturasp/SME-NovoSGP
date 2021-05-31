using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioTurma
    {
        IEnumerable<Turma> MaterializarCodigosTurma(string[] idTurmas, out string[] codigosNaoEncontrados);

        Task<Turma> ObterPorCodigo(string turmaCodigo);

        Task<Turma> ObterPorId(long id);

        Task<Turma> ObterTurmaComUeEDrePorCodigo(string turmaCodigo);

        Task<Turma> ObterTurmaComUeEDrePorId(long turmaId);       

        Task<bool> ObterTurmaEspecialPorCodigo(string turmaCodigo);

        Task<IEnumerable<Turma>> SincronizarAsync(IEnumerable<Turma> entidades, IEnumerable<Ue> ues);

        Task<long> ObterTurmaIdPorCodigo(string turmaCodigo);
        Task<IEnumerable<Turma>> ObterTurmasInfantilNaoDeProgramaPorAnoLetivoAsync(int anoLetivo);
        Task<IEnumerable<string>> ObterCodigosTurmasPorAnoModalidade(int anoLetivo, int[] modalidades, string turmaCodigo = "");
        Task<IEnumerable<TurmaComponenteDto>> ObterTurmasComponentesPorAnoLetivo(DateTime dataReferencia);
        Task<IEnumerable<Turma>> ObterTurmasPorUeModalidadesAno(long ueId, int[] modalidades, int ano);
        Task<IEnumerable<Turma>> ObterTurmasComInicioFechamento(long ueId, long periodoEscolarId, int[] modalidades);
        Task<IEnumerable<Turma>> ObterTurmasPorAnoLetivoModalidade(int anoLetivo, Modalidade[] modalidades);
        Task<IEnumerable<Turma>> ObterTurmasCompletasPorAnoLetivoModalidade(int anoLetivo, Modalidade[] modalidades, string turmaCodigo = "");
        Task<IEnumerable<Turma>> ObterTurmasComFechamentoOuConselhoNaoFinalizados(long ueId, int anoLetivo, long? periodoEscolarId, int[] modalidades, int semestre);
        Task<ObterTurmaSimplesPorIdRetornoDto> ObterTurmaSimplesPorId(long id);
        Task<IEnumerable<Turma>> ObterPorCodigosAsync(string[] codigos);
        Task<IEnumerable<long>> ObterTurmasPorUeAnos(string ueCodigo, int anoLetivo, string[] anos, int modalidadeId);
        Task<Turma> ObterTurmaCompletaPorCodigo(string turmaCodigo);
    }
}
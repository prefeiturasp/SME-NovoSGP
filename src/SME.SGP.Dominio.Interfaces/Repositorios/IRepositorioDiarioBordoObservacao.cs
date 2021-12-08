using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioDiarioBordoObservacao : IRepositorioBase<DiarioBordoObservacao>
    {
        Task<IEnumerable<ListarObservacaoDiarioBordoDto>> ListarPorDiarioBordoAsync(long diarioBordoId, long usuarioLogadoId);
        Task<Turma> ObterTurmaDiarioBordoAulaPorObservacaoId(long observacaoId);
        Task ExcluirObservacoesPorDiarioBordoId(long diarioBordoObservacaoId);
        Task<DiarioBordoObservacaoDto> ObterDiarioBordoObservacaoPorObservacaoId(long observacaoId);
    }
}

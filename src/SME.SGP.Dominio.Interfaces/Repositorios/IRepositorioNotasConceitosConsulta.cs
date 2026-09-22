using System.Collections.Generic;
using System.Threading.Tasks;
using SME.SGP.Infra;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioNotasConceitosConsulta : IRepositorioBase<NotaConceito>
    {
        Task<IEnumerable<NotaEmAprovacaoFechamentoDto>> ObterNotasEmAprovacaoAsync(NotaEmAprovacaoFechamentoDto[] filtros);
        IEnumerable<NotaConceito> ObterNotasPorAlunosAtividadesAvaliativas(IEnumerable<long> atividadesAvaliativas, IEnumerable<string> alunosIds, string disciplinaId);
        Task<IEnumerable<NotaConceito>> ObterNotasPorAlunosAtividadesAvaliativasAsync(long[] atividadesAvaliativasId, string[] alunosIds, string componenteCurricularId);
        Task<IEnumerable<NotaConceito>> ObterNotasPorAlunosAtividadesAvaliativasPorTurmaAsync(string codigoTurma);
        Task<NotaConceito> ObterNotasPorAtividadeIdCodigoAluno(long atividadeId, string codigoAluno);
        Task<NotaConceito> ObterNotasPorId(long id);
        Task<double> ObterNotaEmAprovacao(string codigoAluno, long disciplinaId, long turmaFechamentoId);
    }
}

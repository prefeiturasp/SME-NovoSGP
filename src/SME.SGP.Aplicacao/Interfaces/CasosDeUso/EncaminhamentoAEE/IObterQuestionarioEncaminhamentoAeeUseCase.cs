using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso
{
    public interface IObterQuestionarioEncaminhamentoAeeUseCase
    {
        Task<IEnumerable<QuestaoDto>> Executar(long questionarioId, long? encaminhamentoId, string codigoAluno, string codigoTurma);
    }
}

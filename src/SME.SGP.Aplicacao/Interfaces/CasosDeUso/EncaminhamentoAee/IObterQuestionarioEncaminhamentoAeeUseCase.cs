using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso
{
    public interface IObterQuestionarioEncaminhamentoAeeUseCase
    {
        Task<IEnumerable<QuestaoAeeDto>> Executar(long questionarioId, long? encaminhamentoId, string codigoAluno, string codigoTurma);
    }
}

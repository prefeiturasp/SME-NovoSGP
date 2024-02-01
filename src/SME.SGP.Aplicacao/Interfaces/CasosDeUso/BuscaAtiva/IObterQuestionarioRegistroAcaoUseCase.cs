using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso
{
    public interface IObterQuestionarioRegistroAcaoUseCase
    {
        Task<IEnumerable<QuestaoDto>> Executar(long questionarioId, long? registroAcaoId);
    }
}

using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso.NovoEncaminhamentoNAAPA
{
    public interface IObterQuestionarioNovoEncaminhamentoNAAPAUseCase
    {
        Task<IEnumerable<QuestaoDto>> Executar(long questionarioId, long? encaminhamentoId, string codigoAluno, string codigoTurma);
    }
}
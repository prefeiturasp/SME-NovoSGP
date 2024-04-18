using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso.MapeamentoEstudante
{
    public interface IObterQuestionarioMapeamentoEstudanteUseCase
    {
        Task<IEnumerable<QuestaoDto>> Executar(long questionarioId, long? mapeamentoEstudanteId);
    }
}

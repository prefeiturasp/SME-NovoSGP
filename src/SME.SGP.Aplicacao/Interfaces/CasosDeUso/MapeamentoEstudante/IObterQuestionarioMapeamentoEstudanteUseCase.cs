using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso.MapeamentoEstudante
{
    public interface IObterQuestionarioMapeamentoEstudanteUseCase
    {
        Task<IEnumerable<QuestaoDto>> Executar(FiltroQuestoesQuestionarioMapeamentoEstudanteDto filtro);
    }
}

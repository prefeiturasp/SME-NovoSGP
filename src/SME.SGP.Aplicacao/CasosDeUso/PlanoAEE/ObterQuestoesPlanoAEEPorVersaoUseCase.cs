using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterQuestoesPlanoAEEPorVersaoUseCase : AbstractUseCase, IObterQuestoesPlanoAEEPorVersaoUseCase
    {
        public ObterQuestoesPlanoAEEPorVersaoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<QuestaoDto>> Executar(FiltroPesquisaQuestoesPlanoAEEDto filtro)
            => await mediator.Send(new ObterQuestoesPlanoAEEPorVersaoQuery(filtro.QuestionarioId, filtro.VersaoPlanoId, filtro.TurmaCodigo));
    }
}

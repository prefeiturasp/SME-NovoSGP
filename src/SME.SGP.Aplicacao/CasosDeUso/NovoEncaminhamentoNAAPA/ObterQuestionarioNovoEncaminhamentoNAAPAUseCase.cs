using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.NovoEncaminhamentoNAAPA;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.NovoEncaminhamentoNAAPA
{
    public class ObterQuestionarioNovoEncaminhamentoNAAPAUseCase : IObterQuestionarioNovoEncaminhamentoNAAPAUseCase
    {
        private readonly IMediator mediator;
        public ObterQuestionarioNovoEncaminhamentoNAAPAUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<QuestaoDto>> Executar(long questionarioId, long? encaminhamentoId, string codigoAluno, string codigoTurma)
        {
            return
                await mediator
                .Send(new ObterQuestionarioNovoEncaminhamentoNAAPAQuery(questionarioId, encaminhamentoId, codigoAluno, codigoTurma));
        }
    }
}
using MediatR;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.NovoEncaminhamentoNAAPA.ExcluirRespostaEncaminhamentoNAAPAPorQuestaoId
{
    public class ExcluirRespostaNovoEncaminhamentoNAAPAPorQuestaoIdCommandHandler : IRequestHandler<ExcluirRespostaNovoEncaminhamentoNAAPAPorQuestaoIdCommand, bool>
    {
        public IMediator mediator { get; }
        public IRepositorioRespostaNovoEncaminhamentoNAAPA repositorioRespostaNovoEncaminhamentoNAAPA { get; }

        public ExcluirRespostaNovoEncaminhamentoNAAPAPorQuestaoIdCommandHandler(IMediator mediator, IRepositorioRespostaNovoEncaminhamentoNAAPA repositorioRespostaNovoEncaminhamentoNAAPA)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioRespostaNovoEncaminhamentoNAAPA = repositorioRespostaNovoEncaminhamentoNAAPA ?? throw new ArgumentNullException(nameof(repositorioRespostaNovoEncaminhamentoNAAPA));
        }

        public async Task<bool> Handle(ExcluirRespostaNovoEncaminhamentoNAAPAPorQuestaoIdCommand request, CancellationToken cancellationToken)
        {
            var respostas = await repositorioRespostaNovoEncaminhamentoNAAPA.ObterPorQuestaoEncaminhamentoId(request.QuestaoNovoEncaminhamentoNAAPAId);

            foreach (var resposta in respostas)
                await mediator.Send(new ExcluirRespostaEncaminhamentoNAAPACommand(resposta));

            return true;
        }
    }
}
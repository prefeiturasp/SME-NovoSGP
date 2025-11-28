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

namespace SME.SGP.Aplicacao.Commands.EncaminhamentoNAAPA.ExcluirQuestaoNovoEncaminhamentoNAAPAPorId
{
    public class ExcluirQuestaoNovoEncaminhamentoNAAPAPorIdCommandHandler : IRequestHandler<ExcluirQuestaoNovoEncaminhamentoNAAPAPorIdCommand, bool>
    {
        public IMediator mediator { get; }
        public IRepositorioQuestaoNovoEncaminhamentoNAAPA repositorioQuestaoNovoEncaminhamentoNAAPA { get; }

        public ExcluirQuestaoNovoEncaminhamentoNAAPAPorIdCommandHandler(IMediator mediator, IRepositorioQuestaoNovoEncaminhamentoNAAPA repositorioQuestaoNovoEncaminhamentoNAAPA)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioQuestaoNovoEncaminhamentoNAAPA = repositorioQuestaoNovoEncaminhamentoNAAPA ?? throw new ArgumentNullException(nameof(repositorioQuestaoNovoEncaminhamentoNAAPA));
        }

        public async Task<bool> Handle(ExcluirQuestaoNovoEncaminhamentoNAAPAPorIdCommand request, CancellationToken cancellationToken)
        {
            await repositorioQuestaoNovoEncaminhamentoNAAPA.RemoverLogico(request.QuestaoId);
            await mediator.Send(new ExcluirQuestaoNovoEncaminhamentoNAAPAPorIdCommand(request.QuestaoId));

            return true;
        }
    }
}
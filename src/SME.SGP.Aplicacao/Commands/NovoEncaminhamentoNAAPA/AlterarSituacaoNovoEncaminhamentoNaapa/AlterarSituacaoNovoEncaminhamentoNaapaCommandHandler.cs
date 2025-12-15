using MediatR;
using SME.SGP.Aplicacao.Commands.NovoEncaminhamentoNAAPA.RegistrarHistoricoDeAlteracaoDaSituacaoDoNovoEncaminhamentoNAAPA;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.NovoEncaminhamentoNAAPA.AlterarSituacaoNovoEncaminhamentoNaapa
{
    public class AlterarSituacaoNovoEncaminhamentoNaapaCommandHandler : IRequestHandler<AlterarSituacaoNovoEncaminhamentoNaapaCommand, bool>
    {
        private readonly IRepositorioNovoEncaminhamentoNAAPA repositorioNovoEncaminhamentoNAAPA;
        private readonly IMediator mediator;

        public AlterarSituacaoNovoEncaminhamentoNaapaCommandHandler(IRepositorioNovoEncaminhamentoNAAPA repositorioNovoEncaminhamentoNAAPA, IMediator mediator)
        {
            this.repositorioNovoEncaminhamentoNAAPA = repositorioNovoEncaminhamentoNAAPA ?? throw new ArgumentNullException(nameof(repositorioNovoEncaminhamentoNAAPA));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        public async Task<bool> Handle(AlterarSituacaoNovoEncaminhamentoNaapaCommand request, CancellationToken cancellationToken)
        {
            await mediator.Send(new RegistrarHistoricoDeAlteracaoDaSituacaoDoNovoEncaminhamentoNAAPACommand(request.Encaminhamento, request.Situacao));
            request.Encaminhamento.Situacao = request.Situacao;
            await repositorioNovoEncaminhamentoNAAPA.SalvarAsync(request.Encaminhamento);

            return true;
        }
    }
}
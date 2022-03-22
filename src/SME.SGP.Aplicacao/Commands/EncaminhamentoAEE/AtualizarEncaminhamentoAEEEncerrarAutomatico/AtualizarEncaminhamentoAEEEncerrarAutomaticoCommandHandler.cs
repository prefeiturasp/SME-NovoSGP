using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands
{
    public class AtualizarEncaminhamentoAEEEncerrarAutomaticoCommandHandler : AsyncRequestHandler<AtualizarEncaminhamentoAEEEncerrarAutomaticoCommand>
    {
        private readonly IRepositorioEncaminhamentoAEE _repositorioEncaminhamentoAEE;

        public AtualizarEncaminhamentoAEEEncerrarAutomaticoCommandHandler(IRepositorioEncaminhamentoAEE repositorioEncaminhamentoAEE)
        {
            _repositorioEncaminhamentoAEE = repositorioEncaminhamentoAEE ?? throw new ArgumentNullException(nameof(repositorioEncaminhamentoAEE));
        }

        protected override async Task Handle(AtualizarEncaminhamentoAEEEncerrarAutomaticoCommand request, CancellationToken cancellationToken)
        {
            var encaminhamentoAEE = await _repositorioEncaminhamentoAEE.ObterEncaminhamentoPorId(request.EncaminhamentoId);
            encaminhamentoAEE.EncerrarAutomaticamente();
            await _repositorioEncaminhamentoAEE.SalvarAsync(encaminhamentoAEE);
        }
    }
}

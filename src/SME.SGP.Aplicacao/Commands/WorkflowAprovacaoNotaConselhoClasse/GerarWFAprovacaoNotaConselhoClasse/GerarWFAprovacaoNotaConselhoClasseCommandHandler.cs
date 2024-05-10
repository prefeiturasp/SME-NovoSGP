using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class GerarWFAprovacaoNotaConselhoClasseCommandHandler : AsyncRequestHandler<GerarWFAprovacaoNotaConselhoClasseCommand>
    {
        private readonly IRepositorioWFAprovacaoNotaConselho repositorioWFAprovacaoNotaConselho;
        private readonly IMediator mediator;

        public GerarWFAprovacaoNotaConselhoClasseCommandHandler(IRepositorioWFAprovacaoNotaConselho repositorioWFAprovacaoNotaConselho, IMediator mediator)
        {
            this.repositorioWFAprovacaoNotaConselho = repositorioWFAprovacaoNotaConselho ?? throw new System.ArgumentNullException(nameof(repositorioWFAprovacaoNotaConselho));
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        protected override async Task Handle(GerarWFAprovacaoNotaConselhoClasseCommand request, CancellationToken cancellationToken)
        {
            await ExcluirWorkflowAprovacao(request.ConselhoClasseNotaId);

            await repositorioWFAprovacaoNotaConselho.SalvarAsync(new WFAprovacaoNotaConselho()
            {
                ConselhoClasseNotaId = request.ConselhoClasseNotaId,
                Nota = request.Nota,
                ConceitoId = request.ConceitoId,
                UsuarioSolicitanteId = request.UsuarioLogado.Id,
                ConceitoIdAnterior = request.ConceitoIdAnterior,
                NotaAnterior = request.NotaAnterior
            });
        }

        private async Task ExcluirWorkflowAprovacao(long conselhoClasseNotaId)
        {
            await mediator.Send(new ExcluirWfAprovacaoNotaConselhoPorNotaIdCommand(conselhoClasseNotaId));
        }
    }
}

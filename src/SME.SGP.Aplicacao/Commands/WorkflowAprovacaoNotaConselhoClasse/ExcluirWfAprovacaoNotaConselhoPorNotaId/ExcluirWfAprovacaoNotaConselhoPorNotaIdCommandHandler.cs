﻿using MediatR;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirWfAprovacaoNotaConselhoPorNotaIdCommandHandler : AsyncRequestHandler<ExcluirWfAprovacaoNotaConselhoPorNotaIdCommand>
    {
        private readonly IRepositorioWFAprovacaoNotaConselho repositorioWFAprovacaoNotaConselho;
        private readonly IMediator mediator;

        public ExcluirWfAprovacaoNotaConselhoPorNotaIdCommandHandler(IRepositorioWFAprovacaoNotaConselho repositorioWFAprovacaoNotaConselho, IMediator mediator)
        {
            this.repositorioWFAprovacaoNotaConselho = repositorioWFAprovacaoNotaConselho ?? throw new System.ArgumentNullException(nameof(repositorioWFAprovacaoNotaConselho));
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        protected override async Task Handle(ExcluirWfAprovacaoNotaConselhoPorNotaIdCommand request, CancellationToken cancellationToken)
        {
            var wfAprovacaoNotas = await repositorioWFAprovacaoNotaConselho.ObterWorkflowAprovacaoNota(request.ConselhoClasseNotaId);
            foreach(var wfAprovacaoNota in wfAprovacaoNotas)
                await mediator.Send(new ExcluirWfAprovacaoNotaConselhoClasseCommand(wfAprovacaoNota.Id));
        }
    }
}

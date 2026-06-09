using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirWfAprovacaoNotaConselhoClasseCommandHandler : IRequestHandler<ExcluirWfAprovacaoNotaConselhoClasseCommand>
    {
        private readonly IRepositorioWFAprovacaoNotaConselho repositorioWF;

        public ExcluirWfAprovacaoNotaConselhoClasseCommandHandler(IRepositorioWFAprovacaoNotaConselho repositorioWF)
        {
            this.repositorioWF = repositorioWF ?? throw new ArgumentNullException(nameof(repositorioWF));
        }

        public async Task Handle(ExcluirWfAprovacaoNotaConselhoClasseCommand request, CancellationToken cancellationToken)
        {
            var wfAprovacao = await repositorioWF.ObterPorIdAsync(request.WfAprovacaoConselhoClasseNotaId);
            await repositorioWF.ExcluirLogico(wfAprovacao);
        }
    }
}

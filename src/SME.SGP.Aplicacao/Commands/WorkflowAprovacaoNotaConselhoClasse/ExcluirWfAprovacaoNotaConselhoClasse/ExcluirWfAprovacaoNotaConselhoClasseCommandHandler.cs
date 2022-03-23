using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirWfAprovacaoNotaConselhoClasseCommandHandler : AsyncRequestHandler<ExcluirWfAprovacaoNotaConselhoClasseCommand>
    {
        private readonly IRepositorioWFAprovacaoNotaConselho repositorioWF;

        public ExcluirWfAprovacaoNotaConselhoClasseCommandHandler(IRepositorioWFAprovacaoNotaConselho repositorioWF)
        {
            this.repositorioWF = repositorioWF ?? throw new ArgumentNullException(nameof(repositorioWF));
        }

        protected override async Task Handle(ExcluirWfAprovacaoNotaConselhoClasseCommand request, CancellationToken cancellationToken)
        {
            await repositorioWF.Excluir(request.WfAprovacaoConselhoClasseNotaId);
        }
    }
}

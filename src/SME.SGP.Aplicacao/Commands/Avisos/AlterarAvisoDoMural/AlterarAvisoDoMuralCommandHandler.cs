using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AlterarAvisoDoMuralCommandHandler : AsyncRequestHandler<AlterarAvisoDoMuralCommand>
    {
        private readonly IRepositorioAviso repositorioAviso;

        public AlterarAvisoDoMuralCommandHandler(IRepositorioAviso repositorioAviso)
        {
            this.repositorioAviso = repositorioAviso ?? throw new ArgumentNullException(nameof(repositorioAviso));
        }

        protected override async Task Handle(AlterarAvisoDoMuralCommand request, CancellationToken cancellationToken)
        {
            var aviso = await repositorioAviso.ObterPorIdAsync(request.Id);
            aviso.Mensagem = request.Mensagem;

            await repositorioAviso.SalvarAsync(aviso);
        }
    }
}

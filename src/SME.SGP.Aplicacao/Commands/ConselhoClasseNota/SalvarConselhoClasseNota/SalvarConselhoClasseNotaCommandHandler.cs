using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarConselhoClasseNotaCommandHandler : AsyncRequestHandler<SalvarConselhoClasseNotaCommand>
    {
        private readonly IRepositorioConselhoClasseNota conselhoClasseNota;

        public SalvarConselhoClasseNotaCommandHandler(IRepositorioConselhoClasseNota conselhoClasseNota)
        {
            this.conselhoClasseNota = conselhoClasseNota ?? throw new ArgumentNullException(nameof(conselhoClasseNota));
        }

        protected override async Task Handle(SalvarConselhoClasseNotaCommand request, CancellationToken cancellationToken)
        {
            await conselhoClasseNota.SalvarAsync(request.ConselhoClasseNota);
        }
    }
}

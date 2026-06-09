using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirConselhoClasseNotaCommandHandler : IRequestHandler<ExcluirConselhoClasseNotaCommand>
    {
        private readonly IRepositorioConselhoClasseNota conselhoClasseNota;

        public ExcluirConselhoClasseNotaCommandHandler(IRepositorioConselhoClasseNota conselhoClasseNota)
        {
            this.conselhoClasseNota = conselhoClasseNota ?? throw new ArgumentNullException(nameof(conselhoClasseNota));
        }

        public async Task Handle(ExcluirConselhoClasseNotaCommand request, CancellationToken cancellationToken)
        {
            if (request.SomenteLogico)
                await conselhoClasseNota.RemoverLogico(request.ConselhoClasseNotaId);
            else
                await conselhoClasseNota.Excluir(request.ConselhoClasseNotaId);
        }

    }
}

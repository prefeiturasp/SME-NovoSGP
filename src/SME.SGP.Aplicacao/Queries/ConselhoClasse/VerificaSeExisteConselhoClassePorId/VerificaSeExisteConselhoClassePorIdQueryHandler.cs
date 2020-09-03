
using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class VerificaSeExisteConselhoClassePorIdQueryHandler: IRequestHandler<VerificaSeExisteConselhoClassePorIdQuery, bool>
    {
        private readonly IRepositorioConselhoClasse repositorioConselhoClasse;

        public VerificaSeExisteConselhoClassePorIdQueryHandler(IRepositorioConselhoClasse repositorioConselhoClasse)
        {
            this.repositorioConselhoClasse = repositorioConselhoClasse ?? throw new ArgumentNullException(nameof(repositorioConselhoClasse));
        }

        public async Task<bool> Handle(VerificaSeExisteConselhoClassePorIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioConselhoClasse.Exists(request.ConselhoClasseId);
        }

    }
}

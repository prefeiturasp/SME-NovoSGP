using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterConselhoClassePorFechamentoIdQueryHandler : IRequestHandler<ObterConselhoClassePorFechamentoIdQuery, ConselhoClasse>
    {
        private readonly IRepositorioConselhoClasse repositorioConselhoClasse;

        public ObterConselhoClassePorFechamentoIdQueryHandler(IRepositorioConselhoClasse repositorioConselhoClasse)
        {
            this.repositorioConselhoClasse = repositorioConselhoClasse ?? throw new ArgumentNullException(nameof(repositorioConselhoClasse));
        }

        public async Task<ConselhoClasse> Handle(ObterConselhoClassePorFechamentoIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioConselhoClasse.ObterPorFechamentoId(request.FechamentoId);
        }
    }
}

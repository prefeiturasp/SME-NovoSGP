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
        private readonly IRepositorioConselhoClasseConsulta repositorioConselhoClasseConsulta;

        public ObterConselhoClassePorFechamentoIdQueryHandler(IRepositorioConselhoClasse repositorioConselhoClasse)
        {
            this.repositorioConselhoClasseConsulta = repositorioConselhoClasseConsulta ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseConsulta));
        }

        public async Task<ConselhoClasse> Handle(ObterConselhoClassePorFechamentoIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioConselhoClasseConsulta.ObterPorFechamentoId(request.FechamentoId);
        }
    }
}

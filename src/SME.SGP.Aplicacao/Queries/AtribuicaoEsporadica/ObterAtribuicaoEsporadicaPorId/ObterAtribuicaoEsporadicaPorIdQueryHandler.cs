using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAtribuicaoEsporadicaPorIdQueryHandler : IRequestHandler<ObterAtribuicaoEsporadicaPorIdQuery, AtribuicaoEsporadica>
    {
        private readonly IRepositorioAtribuicaoEsporadica repositorioAtribuicaoEsporadica;

        public ObterAtribuicaoEsporadicaPorIdQueryHandler(IRepositorioAtribuicaoEsporadica repositorioAtribuicaoEsporadica)
        {
            this.repositorioAtribuicaoEsporadica = repositorioAtribuicaoEsporadica ?? throw new System.ArgumentNullException(nameof(repositorioAtribuicaoEsporadica));
        }

        public async Task<AtribuicaoEsporadica> Handle(ObterAtribuicaoEsporadicaPorIdQuery request, CancellationToken cancellationToken)
            => await repositorioAtribuicaoEsporadica.ObterPorIdAsync(request.Id);

    }
}

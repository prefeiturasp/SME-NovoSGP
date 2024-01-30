using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAtribuicoesVigentesPorAnoQueryHandler : IRequestHandler<ObterAtribuicoesVigentesPorAnoQuery, IEnumerable<AtribuicaoEsporadicaVigenteProfDto>>
    {
        private readonly IRepositorioAtribuicaoEsporadica repositorioAtribuicaoEsporadica;

        public ObterAtribuicoesVigentesPorAnoQueryHandler(IRepositorioAtribuicaoEsporadica repositorioAtribuicaoEsporadica)
        {
            this.repositorioAtribuicaoEsporadica = repositorioAtribuicaoEsporadica ?? throw new System.ArgumentNullException(nameof(repositorioAtribuicaoEsporadica));
        }

        public async Task<IEnumerable<AtribuicaoEsporadicaVigenteProfDto>> Handle(ObterAtribuicoesVigentesPorAnoQuery request, CancellationToken cancellationToken)
            => await repositorioAtribuicaoEsporadica.ObterAtribuicoesPorAno(request.AnoLetivo, request.Data);

    }
}

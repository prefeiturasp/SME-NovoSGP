using MediatR;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries
{
    public class ObterSecoesEncaminhamentoDtoPorEtapaQueryHandler : IRequestHandler<ObterSecoesEncaminhamentoDtoPorEtapaQuery, IEnumerable<SecaoQuestionarioDto>>
    {
        private readonly IRepositorioSecaoEncaminhamentoAEE repositorioSecaoAEE;

        public ObterSecoesEncaminhamentoDtoPorEtapaQueryHandler(IRepositorioSecaoEncaminhamentoAEE repositorioSecaoAEE)
        {
            this.repositorioSecaoAEE = repositorioSecaoAEE ?? throw new System.ArgumentNullException(nameof(repositorioSecaoAEE));
        }
        
        public async Task<IEnumerable<SecaoQuestionarioDto>> Handle(ObterSecoesEncaminhamentoDtoPorEtapaQuery request, CancellationToken cancellationToken)
        {
            var secoes = await repositorioSecaoAEE.ObterSecaoEncaminhamentoDtoPorEtapa(new List<int>() { request.Etapa });
            return secoes;
        }
    }
}

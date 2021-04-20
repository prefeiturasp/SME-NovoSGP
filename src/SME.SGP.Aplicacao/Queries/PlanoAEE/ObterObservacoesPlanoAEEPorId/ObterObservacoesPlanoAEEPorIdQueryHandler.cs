using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterObservacoesPlanoAEEPorIdQueryHandler : IRequestHandler<ObterObservacoesPlanoAEEPorIdQuery, IEnumerable<PlanoAEEObservacaoDto>>
    {
        private readonly IRepositorioPlanoAEEObservacao repositorioPlanoAEEObservacao;

        public ObterObservacoesPlanoAEEPorIdQueryHandler(IRepositorioPlanoAEEObservacao repositorioPlanoAEEObservacao)
        {
            this.repositorioPlanoAEEObservacao = repositorioPlanoAEEObservacao ?? throw new ArgumentNullException(nameof(repositorioPlanoAEEObservacao));
        }

        public async Task<IEnumerable<PlanoAEEObservacaoDto>> Handle(ObterObservacoesPlanoAEEPorIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioPlanoAEEObservacao.ObterObservacoesPlanoPorId(request.Id, request.UsuarioRF);
        }
    }
}

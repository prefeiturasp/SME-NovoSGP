using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPlanoAEEReestruturacaoPorIdQueryHandler : IRequestHandler<ObterPlanoAEEReestruturacaoPorIdQuery, PlanoAEEReestruturacao>
    {
        private readonly IRepositorioPlanoAEEReestruturacao repositorioPlanoAEEReestruturacao;

        public ObterPlanoAEEReestruturacaoPorIdQueryHandler(IRepositorioPlanoAEEReestruturacao repositorioPlanoAEEReestruturacao)
        {
            this.repositorioPlanoAEEReestruturacao = repositorioPlanoAEEReestruturacao ?? throw new ArgumentNullException(nameof(repositorioPlanoAEEReestruturacao));
        }

        public async Task<PlanoAEEReestruturacao> Handle(ObterPlanoAEEReestruturacaoPorIdQuery request, CancellationToken cancellationToken)
            => await repositorioPlanoAEEReestruturacao.ObterCompletoPorId(request.ReestruturacaoId);
    }
}

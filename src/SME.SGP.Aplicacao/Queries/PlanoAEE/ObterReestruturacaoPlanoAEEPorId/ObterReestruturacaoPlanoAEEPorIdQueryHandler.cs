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
    public class ObterReestruturacaoPlanoAEEPorIdQueryHandler : IRequestHandler<ObterReestruturacaoPlanoAEEPorIdQuery, PlanoAEEReestruturacao>
    {
        private readonly IRepositorioPlanoAEEReestruturacao repositorioPlanoAEEReestruturacao;

        public ObterReestruturacaoPlanoAEEPorIdQueryHandler(IRepositorioPlanoAEEReestruturacao repositorioPlanoAEEReestruturacao)
        {
            this.repositorioPlanoAEEReestruturacao = repositorioPlanoAEEReestruturacao ?? throw new ArgumentNullException(nameof(repositorioPlanoAEEReestruturacao));
        }

        public async Task<PlanoAEEReestruturacao> Handle(ObterReestruturacaoPlanoAEEPorIdQuery request, CancellationToken cancellationToken)
            => await repositorioPlanoAEEReestruturacao.ObterPorIdAsync(request.Id);
    }
}

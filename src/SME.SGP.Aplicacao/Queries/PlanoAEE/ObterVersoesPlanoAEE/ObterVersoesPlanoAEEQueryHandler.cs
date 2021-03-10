using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.PlanoAEE.ObterVersoesPlanoAEE
{
    public class ObterVersoesPlanoAEEQueryHandler : IRequestHandler<ObterVersoesPlanoAEEQuery, IEnumerable<PlanoAEEVersaoDto>>
    {
        private readonly IRepositorioPlanoAEEVersao repositorioPlanoAEEVersao;

        public ObterVersoesPlanoAEEQueryHandler(IRepositorioPlanoAEEVersao repositorioPlanoAEEVersao)
        {
            this.repositorioPlanoAEEVersao = repositorioPlanoAEEVersao ?? throw new ArgumentNullException(nameof(repositorioPlanoAEEVersao));

        }

        public async Task<IEnumerable<PlanoAEEVersaoDto>> Handle(ObterVersoesPlanoAEEQuery request, CancellationToken cancellationToken)
            => await repositorioPlanoAEEVersao.ObterVersoesPorPlanoId(request.PlanoId);
    }
}

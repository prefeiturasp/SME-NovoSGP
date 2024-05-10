using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterVersoesPlanoAEEPorCodigoEstudanteQueryHandler : IRequestHandler<ObterVersoesPlanoAEEPorCodigoEstudanteQuery, IEnumerable<PlanoAEEVersaoDto>>
    {
        private readonly IRepositorioPlanoAEEVersao repositorioPlanoAEEVersao;

        public ObterVersoesPlanoAEEPorCodigoEstudanteQueryHandler(IRepositorioPlanoAEEVersao repositorioPlanoAEEVersao)
        {
            this.repositorioPlanoAEEVersao = repositorioPlanoAEEVersao ?? throw new ArgumentNullException(nameof(repositorioPlanoAEEVersao));
        }

        public async Task<IEnumerable<PlanoAEEVersaoDto>> Handle(ObterVersoesPlanoAEEPorCodigoEstudanteQuery request, CancellationToken cancellationToken)
            => await repositorioPlanoAEEVersao.ObterVersoesPorCodigoEstudante(request.CodigoEstudante);
    }
}

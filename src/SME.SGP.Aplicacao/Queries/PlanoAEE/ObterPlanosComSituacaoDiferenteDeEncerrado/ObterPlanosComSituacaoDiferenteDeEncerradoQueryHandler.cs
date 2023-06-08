using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPlanosComSituacaoDiferenteDeEncerradoQueryHandler : IRequestHandler<ObterPlanosComSituacaoDiferenteDeEncerradoQuery, IEnumerable<PlanoAEETurmaDto>>
    {
        private readonly IRepositorioPlanoAEE repositorioPlano;

        public ObterPlanosComSituacaoDiferenteDeEncerradoQueryHandler(IRepositorioPlanoAEE repositorioPlano)
        {
            this.repositorioPlano = repositorioPlano ?? throw new ArgumentNullException(nameof(repositorioPlano));
        }

        public Task<IEnumerable<PlanoAEETurmaDto>> Handle(ObterPlanosComSituacaoDiferenteDeEncerradoQuery request, CancellationToken cancellationToken)
        {
            return this.repositorioPlano.ObterPlanosComSituacaoDiferenteDeEncerrado();
        }
    }
}

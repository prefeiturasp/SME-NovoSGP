using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPlanosAEEAtivosComTurmaEVigenciaQueryHandler : IRequestHandler<ObterPlanosAEEAtivosComTurmaEVigenciaQuery, IEnumerable<PlanoAEEReduzidoDto>>
    {
        private readonly IRepositorioPlanoAEE repositorioPlanoAEE;

        public ObterPlanosAEEAtivosComTurmaEVigenciaQueryHandler(IRepositorioPlanoAEE repositorioPlanoAEE)
        {
            this.repositorioPlanoAEE = repositorioPlanoAEE ?? throw new ArgumentNullException(nameof(repositorioPlanoAEE));
        }

        public async Task<IEnumerable<PlanoAEEReduzidoDto>> Handle(ObterPlanosAEEAtivosComTurmaEVigenciaQuery request, CancellationToken cancellationToken)
            => await repositorioPlanoAEE.ObterPlanosAEEAtivosComTurmaEVigencia();
    }
}

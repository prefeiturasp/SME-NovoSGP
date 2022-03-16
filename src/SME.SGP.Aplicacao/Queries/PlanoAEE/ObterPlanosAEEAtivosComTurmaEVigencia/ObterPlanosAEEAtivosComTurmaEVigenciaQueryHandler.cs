using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPlanosAEEAtivosComTurmaEVigenciaQueryHandler : IRequestHandler<ObterPlanosAEEAtivosComTurmaEVigenciaQuery, IEnumerable<PlanoAEEReduzidoDto>>
    {
        private readonly IRepositorioPlanoAEEConsulta repositorioPlanoAEE;

        public ObterPlanosAEEAtivosComTurmaEVigenciaQueryHandler(IRepositorioPlanoAEEConsulta repositorioPlanoAEE)
        {
            this.repositorioPlanoAEE = repositorioPlanoAEE ?? throw new ArgumentNullException(nameof(repositorioPlanoAEE));
        }

        public async Task<IEnumerable<PlanoAEEReduzidoDto>> Handle(ObterPlanosAEEAtivosComTurmaEVigenciaQuery request, CancellationToken cancellationToken)
            => await repositorioPlanoAEE.ObterPlanosAEEAtivosComTurmaEVigencia();
    }
}

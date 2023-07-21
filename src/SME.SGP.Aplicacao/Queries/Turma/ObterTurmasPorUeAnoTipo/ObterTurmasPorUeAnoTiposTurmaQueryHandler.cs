using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasPorUeAnoTiposTurmaQueryHandler : IRequestHandler<ObterTurmasPorUeAnoTiposTurmaQuery, IEnumerable<TurmaDTO>>
    {
        private readonly IRepositorioTurmaConsulta repositorioTurmaConsulta;

        public ObterTurmasPorUeAnoTiposTurmaQueryHandler(IRepositorioTurmaConsulta repositorioTurmaConsulta)
        {
            this.repositorioTurmaConsulta = repositorioTurmaConsulta ?? throw new ArgumentNullException(nameof(repositorioTurmaConsulta));
        }

        public Task<IEnumerable<TurmaDTO>> Handle(ObterTurmasPorUeAnoTiposTurmaQuery request, CancellationToken cancellationToken)
        {
            return repositorioTurmaConsulta.ObterTurmasPorUeAnoTiposTurma(request.UeId, request.AnoLetivo, request.TiposTurma);
        }
    }
}

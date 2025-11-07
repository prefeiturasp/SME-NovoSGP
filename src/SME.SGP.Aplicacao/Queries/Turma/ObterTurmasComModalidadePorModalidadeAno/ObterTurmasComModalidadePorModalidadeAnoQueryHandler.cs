using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasComModalidadePorModalidadeAnoQueryHandler : IRequestHandler<ObterTurmasComModalidadePorModalidadeAnoQuery, IEnumerable<TurmaModalidadeSerieAnoDto>>
    {
        private readonly IRepositorioTurmaConsulta repositorio;

        public ObterTurmasComModalidadePorModalidadeAnoQueryHandler(IRepositorioTurmaConsulta repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<IEnumerable<TurmaModalidadeSerieAnoDto>> Handle(ObterTurmasComModalidadePorModalidadeAnoQuery request, CancellationToken cancellationToken)
        {
            return await repositorio.ObterTurmasComModalidadePorModalidadeAnoUe(request.Ano, [.. request.UeId], [.. request.Modalidades]);
        }
    }
}

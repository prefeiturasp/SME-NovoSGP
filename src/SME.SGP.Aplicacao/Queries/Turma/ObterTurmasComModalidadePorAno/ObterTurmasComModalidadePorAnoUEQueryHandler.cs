using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasComModalidadePorAnoUEQueryHandler : IRequestHandler<ObterTurmasComModalidadePorAnoUEQuery, IEnumerable<TurmaModalidadeDto>>
    {
        private readonly IRepositorioTurmaConsulta repositorio;

        public ObterTurmasComModalidadePorAnoUEQueryHandler(IRepositorioTurmaConsulta repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<IEnumerable<TurmaModalidadeDto>> Handle(ObterTurmasComModalidadePorAnoUEQuery request, CancellationToken cancellationToken)
            => await repositorio.ObterTurmasComModalidadePorAnoUe(request.Ano, request.UeId);
    }
}

using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasAulasNormaisQueryHandler : IRequestHandler<ObterTurmasAulasNormaisQuery, IEnumerable<TurmaDTO>>
    {
        private readonly IRepositorioTurmaConsulta repositorioTurmaConsulta;

        public ObterTurmasAulasNormaisQueryHandler(IRepositorioTurmaConsulta repositorioTurmaConsulta)
        {
            this.repositorioTurmaConsulta = repositorioTurmaConsulta ?? throw new ArgumentNullException(nameof(repositorioTurmaConsulta));
        }

        public Task<IEnumerable<TurmaDTO>> Handle(ObterTurmasAulasNormaisQuery request, CancellationToken cancellationToken)
        {
            return repositorioTurmaConsulta.ObterTurmasAulasNormais(request.UeId, request.AnoLetivo, request.TiposTurma, request.Modalidades, request.IgnorarTiposCiclos);
        }
    }
}

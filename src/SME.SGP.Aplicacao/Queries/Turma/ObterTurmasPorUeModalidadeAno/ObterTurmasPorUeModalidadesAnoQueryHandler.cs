using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasPorUeModalidadesAnoQueryHandler : IRequestHandler<ObterTurmasPorUeModalidadesAnoQuery, IEnumerable<Turma>>
    {
        private readonly IRepositorioTurmaConsulta repositorioTurmaConsulta;

        public ObterTurmasPorUeModalidadesAnoQueryHandler(IRepositorioTurmaConsulta repositorioTurmaConsulta)
        {
            this.repositorioTurmaConsulta = repositorioTurmaConsulta ?? throw new ArgumentNullException(nameof(repositorioTurmaConsulta));
        }

        public async Task<IEnumerable<Turma>> Handle(ObterTurmasPorUeModalidadesAnoQuery request, CancellationToken cancellationToken)
            => await repositorioTurmaConsulta.ObterTurmasPorUeModalidadesAno(request.UeId, request.Modalidades.Cast<int>().ToArray(), request.Ano);
    }
}

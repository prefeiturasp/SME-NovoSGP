using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTotalCompensacoesPorTurmaBimestresDisciplinasQueryHandler : IRequestHandler<ObterTotalCompensacoesPorTurmaBimestresDisciplinasQuery, IEnumerable<AlunoDisciplinaTotalCompensacaoAusenciaDto>>
    {
        private readonly IRepositorioCompensacaoAusenciaAlunoConsulta repositorio;

        public ObterTotalCompensacoesPorTurmaBimestresDisciplinasQueryHandler(IRepositorioCompensacaoAusenciaAlunoConsulta repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public Task<IEnumerable<AlunoDisciplinaTotalCompensacaoAusenciaDto>> Handle(ObterTotalCompensacoesPorTurmaBimestresDisciplinasQuery request, CancellationToken cancellationToken)
            => repositorio.ObterTotalCompensacoesPorTurmaBimestresDisciplinas(request.Bimestres, request.DisciplinasIds, request.TurmaCodigo);
    }
}

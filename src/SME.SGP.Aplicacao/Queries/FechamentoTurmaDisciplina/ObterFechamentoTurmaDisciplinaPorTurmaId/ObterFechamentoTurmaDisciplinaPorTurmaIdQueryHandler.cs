using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFechamentoTurmaDisciplinaPorTurmaIdQueryHandler : IRequestHandler<ObterFechamentoTurmaDisciplinaPorTurmaIdQuery, IEnumerable<TurmaFechamentoDisciplinaSituacaoDto>>
    {
        private readonly IRepositorioFechamentoTurmaDisciplinaConsulta repositorioFechamentoTurmaDisciplina;

        public ObterFechamentoTurmaDisciplinaPorTurmaIdQueryHandler(IRepositorioFechamentoTurmaDisciplinaConsulta repositorio)
        {
            this.repositorioFechamentoTurmaDisciplina = repositorio;
        }
        public async  Task<IEnumerable<TurmaFechamentoDisciplinaSituacaoDto>> Handle(ObterFechamentoTurmaDisciplinaPorTurmaIdQuery request, CancellationToken cancellationToken)
            => await repositorioFechamentoTurmaDisciplina.ObterFechamentosTurmaPorTurmaId(request.TurmaId);
    }
}

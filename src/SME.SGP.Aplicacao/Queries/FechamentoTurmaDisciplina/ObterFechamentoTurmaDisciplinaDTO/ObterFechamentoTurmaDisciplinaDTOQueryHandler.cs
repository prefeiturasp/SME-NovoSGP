using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterFechamentoTurmaDisciplinaDTOQueryHandler : IRequestHandler<ObterFechamentoTurmaDisciplinaDTOQuery, FechamentoTurmaDisciplinaPendenciaDto>
    {
        private readonly IRepositorioFechamentoTurmaDisciplinaConsulta repositorioFechamentoTurmaDisciplina;

        public ObterFechamentoTurmaDisciplinaDTOQueryHandler(IRepositorioFechamentoTurmaDisciplinaConsulta repositorioFechamentoTurmaDisciplina)
        {
            this.repositorioFechamentoTurmaDisciplina = repositorioFechamentoTurmaDisciplina ?? throw new ArgumentNullException(nameof(repositorioFechamentoTurmaDisciplina));
        }

        public Task<FechamentoTurmaDisciplinaPendenciaDto> Handle(ObterFechamentoTurmaDisciplinaDTOQuery request, CancellationToken cancellationToken)
            => repositorioFechamentoTurmaDisciplina.ObterFechamentoTurmaDisciplinaDTOPorTurmaDisciplinaBimestre(request.TurmaCodigo, request.DisciplinaId, request.Bimestre, request.SituacoesFechamento);
    }
}

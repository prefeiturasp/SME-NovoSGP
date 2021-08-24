using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Dtos;

namespace SME.SGP.Aplicacao
{
    public class ObterTotalDisciplinasFechamentoPorTurmaQueryHandler : IRequestHandler<ObterTotalDisciplinasFechamentoPorTurmaQuery, IEnumerable<TurmaFechamentoDisciplinaDto>>
    {
        private readonly IRepositorioFechamentoTurmaDisciplina repositorio;

        public ObterTotalDisciplinasFechamentoPorTurmaQueryHandler(IRepositorioFechamentoTurmaDisciplina repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<IEnumerable<TurmaFechamentoDisciplinaDto>> Handle(ObterTotalDisciplinasFechamentoPorTurmaQuery request,
            CancellationToken cancellationToken)
            => await repositorio.ObterTotalDisciplinasPorTurma(request.AnoLetivo, request.Bimestre);
    }
}
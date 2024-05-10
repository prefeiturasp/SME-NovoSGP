using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTotalCompensacoesAlunosETurmaPorPeriodoQueryHandler : IRequestHandler<ObterTotalCompensacoesAlunosETurmaPorPeriodoQuery, IEnumerable<CompensacaoAusenciaAlunoCalculoFrequenciaDto>>
    {
        private readonly IRepositorioCompensacaoAusenciaAlunoConsulta repositorio;

        public ObterTotalCompensacoesAlunosETurmaPorPeriodoQueryHandler(IRepositorioCompensacaoAusenciaAlunoConsulta repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public Task<IEnumerable<CompensacaoAusenciaAlunoCalculoFrequenciaDto>> Handle(ObterTotalCompensacoesAlunosETurmaPorPeriodoQuery request, CancellationToken cancellationToken)
            => repositorio.ObterTotalCompensacoesPorAlunosETurmaAsync(request.Bimestre, request.Alunos, request.TurmaCodigo, request.Professor);
    }
}

using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTotalCompensacoesAusenciaPorBimestreTurmaAlunosDesconsiderandoIdQueryHandler : IRequestHandler<ObterTotalCompensacoesAusenciaPorBimestreTurmaAlunosDesconsiderandoIdQuery, IEnumerable<CompensacaoAusenciaAlunoCalculoFrequenciaDto>>
    {
        private readonly IRepositorioCompensacaoAusenciaAlunoConsulta repositorioCompensacaoAusenciaAlunoConsulta;

        public ObterTotalCompensacoesAusenciaPorBimestreTurmaAlunosDesconsiderandoIdQueryHandler(IRepositorioCompensacaoAusenciaAlunoConsulta repositorioCompensacaoAusenciaAlunoConsulta)
        {
            this.repositorioCompensacaoAusenciaAlunoConsulta = repositorioCompensacaoAusenciaAlunoConsulta ?? throw new ArgumentNullException(nameof(repositorioCompensacaoAusenciaAlunoConsulta));
        }

        public Task<IEnumerable<CompensacaoAusenciaAlunoCalculoFrequenciaDto>> Handle(ObterTotalCompensacoesAusenciaPorBimestreTurmaAlunosDesconsiderandoIdQuery request, CancellationToken cancellationToken)
        {
            return repositorioCompensacaoAusenciaAlunoConsulta
                .ObterTotalCompensacoesPorAlunosETurmaDesconsiderandoIdCompensacaoAsync(request.Bimestre, request.CodigoTurma, request.CodigosAlunos, request.IdCompensacaoDesconsiderado);
        }
    }
}

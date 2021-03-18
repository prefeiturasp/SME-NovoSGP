using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAcompanhamentoPorAlunoTurmaESemestreQueryHandler : IRequestHandler<ObterAcompanhamentoPorAlunoTurmaESemestreQuery, IEnumerable<AcompanhamentoAlunoTurmaSemestreDto>>
    {
        private readonly IRepositorioAcompanhamentoAluno repositorioAcompanhamentoAluno;

        public ObterAcompanhamentoPorAlunoTurmaESemestreQueryHandler(IRepositorioAcompanhamentoAluno repositorioAcompanhamentoAluno)
        {
            this.repositorioAcompanhamentoAluno = repositorioAcompanhamentoAluno ?? throw new ArgumentNullException(nameof(repositorioAcompanhamentoAluno));
        }

        public async Task<IEnumerable<AcompanhamentoAlunoTurmaSemestreDto>> Handle(ObterAcompanhamentoPorAlunoTurmaESemestreQuery request, CancellationToken cancellationToken)
            => await repositorioAcompanhamentoAluno.ObterAcompanhamentoPorTurmaAlunoESemestre(request.TurmaCodigo, request.AlunoCodigo, request.Semestre);
    }
}

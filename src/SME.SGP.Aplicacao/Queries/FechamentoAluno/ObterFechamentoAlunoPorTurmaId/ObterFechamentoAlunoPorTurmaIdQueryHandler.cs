using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFechamentoAlunoPorTurmaIdQueryHandler : IRequestHandler<ObterFechamentoAlunoPorTurmaIdQuery, FechamentoAluno>
    {
        private readonly IRepositorioFechamentoAlunoConsulta repositorioFechamentoAluno;

        public ObterFechamentoAlunoPorTurmaIdQueryHandler(IRepositorioFechamentoAlunoConsulta repositorioFechamentoAluno)
        {
            this.repositorioFechamentoAluno = repositorioFechamentoAluno ?? throw new ArgumentNullException(nameof(repositorioFechamentoAluno));
        }

        public async Task<FechamentoAluno> Handle(ObterFechamentoAlunoPorTurmaIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioFechamentoAluno.ObterFechamentoAlunoENotas(request.FechamentoTurmaDisciplinaId, request.AlunoCodigo);
        }
    }
}

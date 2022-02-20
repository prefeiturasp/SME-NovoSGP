using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAnotacaoFechamentoAlunoPorFechamentoEAlunoQueryHandler : IRequestHandler<ObterAnotacaoFechamentoAlunoPorFechamentoEAlunoQuery, AnotacaoFechamentoAluno>
    {
        private readonly IRepositorioAnotacaoFechamentoAluno repositorioAnotacaoFechamentoAluno;

        public ObterAnotacaoFechamentoAlunoPorFechamentoEAlunoQueryHandler(IRepositorioAnotacaoFechamentoAluno repositorioAnotacaoFechamentoAluno)
        {
            this.repositorioAnotacaoFechamentoAluno = repositorioAnotacaoFechamentoAluno ?? throw new ArgumentNullException(nameof(repositorioAnotacaoFechamentoAluno));
        }

        public Task<AnotacaoFechamentoAluno> Handle(ObterAnotacaoFechamentoAlunoPorFechamentoEAlunoQuery request, CancellationToken cancellationToken)
            => repositorioAnotacaoFechamentoAluno.ObterPorFechamentoEAluno(request.FechamentoTurmaDisciplinaId, request.AlunoCodigo);
    }
}

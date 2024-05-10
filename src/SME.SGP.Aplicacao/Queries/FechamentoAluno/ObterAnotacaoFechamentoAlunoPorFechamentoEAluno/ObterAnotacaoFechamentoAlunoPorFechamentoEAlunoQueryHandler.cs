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
        private readonly IRepositorioAnotacaoFechamentoAlunoConsulta repositorioAnotacaoFechamentoAlunoConsulta;

        public ObterAnotacaoFechamentoAlunoPorFechamentoEAlunoQueryHandler(IRepositorioAnotacaoFechamentoAlunoConsulta repositorioAnotacaoFechamentoAlunoConsulta)
        {
            this.repositorioAnotacaoFechamentoAlunoConsulta = repositorioAnotacaoFechamentoAlunoConsulta ?? throw new ArgumentNullException(nameof(repositorioAnotacaoFechamentoAlunoConsulta));
        }

        public Task<AnotacaoFechamentoAluno> Handle(ObterAnotacaoFechamentoAlunoPorFechamentoEAlunoQuery request, CancellationToken cancellationToken)
            => repositorioAnotacaoFechamentoAlunoConsulta.ObterPorFechamentoEAluno(request.FechamentoTurmaDisciplinaId, request.AlunoCodigo);
    }
}

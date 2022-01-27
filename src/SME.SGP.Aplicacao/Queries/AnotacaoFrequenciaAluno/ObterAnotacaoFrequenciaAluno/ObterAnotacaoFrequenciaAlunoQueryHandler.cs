using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAnotacaoFrequenciaAlunoQueryHandler : IRequestHandler<ObterAnotacaoFrequenciaAlunoQuery, AnotacaoFrequenciaAluno>
    {
        private readonly IRepositorioAnotacaoFrequenciaAlunoConsulta repositorioAnotacaoFrequenciaAluno;

        public ObterAnotacaoFrequenciaAlunoQueryHandler(IRepositorioAnotacaoFrequenciaAlunoConsulta repositorioAnotacaoFrequenciaAluno)
        {
            this.repositorioAnotacaoFrequenciaAluno = repositorioAnotacaoFrequenciaAluno ?? throw new ArgumentNullException(nameof(repositorioAnotacaoFrequenciaAluno));
        }

        public async Task<AnotacaoFrequenciaAluno> Handle(ObterAnotacaoFrequenciaAlunoQuery request, CancellationToken cancellationToken)
            => await repositorioAnotacaoFrequenciaAluno.ObterPorAlunoAula(request.CodigoAluno, request.AulaId);
    }
}

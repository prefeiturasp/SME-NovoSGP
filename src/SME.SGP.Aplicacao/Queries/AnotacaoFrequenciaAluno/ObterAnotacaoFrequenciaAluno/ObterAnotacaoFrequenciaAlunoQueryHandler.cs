using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAnotacaoFrequenciaAlunoQueryHandler : IRequestHandler<ObterAnotacaoFrequenciaAlunoQuery, AnotacaoFrequenciaAluno>
    {
        private readonly IRepositorioAnotacaoFrequenciaAluno repositorioAnotacaoFrequenciaAluno;

        public ObterAnotacaoFrequenciaAlunoQueryHandler(IRepositorioAnotacaoFrequenciaAluno repositorioAnotacaoFrequenciaAluno)
        {
            this.repositorioAnotacaoFrequenciaAluno = repositorioAnotacaoFrequenciaAluno ?? throw new ArgumentNullException(nameof(repositorioAnotacaoFrequenciaAluno));
        }

        public async Task<AnotacaoFrequenciaAluno> Handle(ObterAnotacaoFrequenciaAlunoQuery request, CancellationToken cancellationToken)
            => await repositorioAnotacaoFrequenciaAluno.ObterPorAlunoAula(request.CodigoAluno, request.AulaId);
    }
}

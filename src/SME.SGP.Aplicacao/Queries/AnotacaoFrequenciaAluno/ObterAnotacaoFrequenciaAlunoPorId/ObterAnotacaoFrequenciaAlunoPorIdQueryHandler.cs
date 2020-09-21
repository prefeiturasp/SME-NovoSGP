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
    public class ObterAnotacaoFrequenciaAlunoPorIdQueryHandler : IRequestHandler<ObterAnotacaoFrequenciaAlunoPorIdQuery, AnotacaoFrequenciaAluno>
    {
        private readonly IRepositorioAnotacaoFrequenciaAluno repositorioAnotacaoFrequenciaAluno;

        public ObterAnotacaoFrequenciaAlunoPorIdQueryHandler(IRepositorioAnotacaoFrequenciaAluno repositorioAnotacaoFrequenciaAluno)
        {
            this.repositorioAnotacaoFrequenciaAluno = repositorioAnotacaoFrequenciaAluno ?? throw new ArgumentNullException(nameof(repositorioAnotacaoFrequenciaAluno));
        }

        public async Task<AnotacaoFrequenciaAluno> Handle(ObterAnotacaoFrequenciaAlunoPorIdQuery request, CancellationToken cancellationToken)
            => await repositorioAnotacaoFrequenciaAluno.ObterPorIdAsync(request.Id);
    }
}

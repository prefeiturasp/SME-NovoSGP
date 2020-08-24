using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunosComAnotacaoNaAulaQueryHandler : IRequestHandler<ObterAlunosComAnotacaoNaAulaQuery, IEnumerable<string>>
    {
        private readonly IRepositorioAnotacaoFrequenciaAluno repositorioAnotacaoFrequenciaAluno;

        public ObterAlunosComAnotacaoNaAulaQueryHandler(IRepositorioAnotacaoFrequenciaAluno repositorioAnotacaoFrequenciaAluno)
        {
            this.repositorioAnotacaoFrequenciaAluno = repositorioAnotacaoFrequenciaAluno ?? throw new ArgumentNullException(nameof(repositorioAnotacaoFrequenciaAluno));
        }

        public async Task<IEnumerable<string>> Handle(ObterAlunosComAnotacaoNaAulaQuery request, CancellationToken cancellationToken)
            => await repositorioAnotacaoFrequenciaAluno.ListarAlunosComAnotacaoFrequenciaNaAula(request.AulaId);
    }
}

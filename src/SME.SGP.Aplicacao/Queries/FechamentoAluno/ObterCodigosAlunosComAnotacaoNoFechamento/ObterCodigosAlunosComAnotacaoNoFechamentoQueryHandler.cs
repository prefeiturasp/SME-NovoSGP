using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterCodigosAlunosComAnotacaoNoFechamentoQueryHandler : IRequestHandler<ObterCodigosAlunosComAnotacaoNoFechamentoQuery, IEnumerable<string>>
    {
        private readonly IRepositorioAnotacaoFechamentoAluno repositorioAnotacaoFechamentoAluno;

        public ObterCodigosAlunosComAnotacaoNoFechamentoQueryHandler(IRepositorioAnotacaoFechamentoAluno repositorioAnotacaoFechamentoAluno)
        {
            this.repositorioAnotacaoFechamentoAluno = repositorioAnotacaoFechamentoAluno ?? throw new ArgumentNullException(nameof(repositorioAnotacaoFechamentoAluno));
        }

        public Task<IEnumerable<string>> Handle(ObterCodigosAlunosComAnotacaoNoFechamentoQuery request, CancellationToken cancellationToken)
            => repositorioAnotacaoFechamentoAluno.ObterAlunosComAnotacaoNoFechamento(request.FechamentoTurmaDisciplinaId);
    }
}

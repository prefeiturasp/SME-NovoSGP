using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterObservacoesDosAlunosNoHistoricoEscolarQueryHandler : IRequestHandler<ObterObservacoesDosAlunosNoHistoricoEscolarQuery, IEnumerable<Dominio.HistoricoEscolarObservacao>>
    {
        private readonly IRepositorioHistoricoEscolarObservacao repositorioHistoricoEscolarObservacao;

        public ObterObservacoesDosAlunosNoHistoricoEscolarQueryHandler(IRepositorioHistoricoEscolarObservacao repositorioHistoricoEscolarObservacao)
        {
            this.repositorioHistoricoEscolarObservacao = repositorioHistoricoEscolarObservacao ?? throw new ArgumentNullException(nameof(repositorioHistoricoEscolarObservacao));
        }

        public Task<IEnumerable<HistoricoEscolarObservacao>> Handle(ObterObservacoesDosAlunosNoHistoricoEscolarQuery request, CancellationToken cancellationToken)
        {
            return repositorioHistoricoEscolarObservacao.ObterPorCodigosAlunosAsync(request.CodigosAlunos);
        }
    }
}

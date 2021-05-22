using MediatR;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTokenAtualQueryHandler : IRequestHandler<ObterTokenAtualQuery, string>
    {
        private readonly IContextoAplicacao contextoAplicacao;

        public ObterTokenAtualQueryHandler(IContextoAplicacao contextoAplicacao)
        {
            this.contextoAplicacao = contextoAplicacao ?? throw new ArgumentNullException(nameof(contextoAplicacao));
        }

        public async Task<string> Handle(ObterTokenAtualQuery request, CancellationToken cancellationToken)
        {
            return contextoAplicacao.ObterVariavel<string>("TokenAtual");
        }
    }
}

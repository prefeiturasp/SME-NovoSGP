using MediatR;
using SME.SGP.Infra.Interfaces;
using System;
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

        public Task<string> Handle(ObterTokenAtualQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(contextoAplicacao.ObterVariavel<string>("TokenAtual"));
        }
    }
}

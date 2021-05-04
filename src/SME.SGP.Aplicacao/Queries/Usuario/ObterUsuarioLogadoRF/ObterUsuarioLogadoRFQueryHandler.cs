using MediatR;
using SME.SGP.Infra.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUsuarioLogadoRFQueryHandler : IRequestHandler<ObterUsuarioLogadoRFQuery, string>
    {
        private readonly IContextoAplicacao contextoAplicacao;

        public ObterUsuarioLogadoRFQueryHandler(IContextoAplicacao contextoAplicacao)
        {
            this.contextoAplicacao = contextoAplicacao ?? throw new ArgumentNullException(nameof(contextoAplicacao));
        }

        public Task<string> Handle(ObterUsuarioLogadoRFQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(contextoAplicacao.UsuarioLogado);
        }



    }
}

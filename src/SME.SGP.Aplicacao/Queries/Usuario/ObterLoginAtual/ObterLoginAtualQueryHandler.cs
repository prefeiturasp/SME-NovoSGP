using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterLoginAtualQueryHandler : IRequestHandler<ObterLoginAtualQuery, string>
    {
        private readonly IContextoAplicacao contextoAplicacao;

        public ObterLoginAtualQueryHandler(IContextoAplicacao contextoAplicacao)
        {
            this.contextoAplicacao = contextoAplicacao ?? throw new ArgumentNullException(nameof(contextoAplicacao));
        }

        public Task<string> Handle(ObterLoginAtualQuery request, CancellationToken cancellationToken)
        {
            var loginAtual = contextoAplicacao.ObterVariavel<string>("login");
            if (loginAtual == null)
                throw new NegocioException("Não foi possível localizar o login no token");

            return Task.FromResult(loginAtual);
        }
    }
}

using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra.Contexto;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUsuarioLogadoRFQueryHandler : IRequestHandler<ObterUsuarioLogadoRFQuery, string>
    {
        private readonly IContextoAplicacao contextoAplicacao;
        private const string CLAIM_RF = "rf";

        public ObterUsuarioLogadoRFQueryHandler(IContextoAplicacao contextoAplicacao)
        {
            this.contextoAplicacao = contextoAplicacao ?? throw new ArgumentNullException(nameof(contextoAplicacao));
        }
        public Task<string> Handle(ObterUsuarioLogadoRFQuery request, CancellationToken cancellationToken)
        {
            var rf = contextoAplicacao.ObterVarivel<IEnumerable<InternalClaim>>("Claims").FirstOrDefault(a => a.Type == CLAIM_RF);
            if (rf == null)
                throw new NegocioException("Não foi possível obter o rf do usuário logado.");

            return Task.FromResult(rf.Value);
        }
    }
}

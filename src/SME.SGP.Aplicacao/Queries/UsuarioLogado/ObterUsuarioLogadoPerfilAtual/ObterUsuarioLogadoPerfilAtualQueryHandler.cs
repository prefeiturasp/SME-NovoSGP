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
    public class ObterUsuarioLogadoPerfilAtualQueryHandler : IRequestHandler<ObterUsuarioLogadoPerfilAtualQuery, Guid>
    {
        private readonly IContextoAplicacao contextoAplicacao;
        private const string CLAIM_PERFIL_ATUAL = "perfil";

        public ObterUsuarioLogadoPerfilAtualQueryHandler(IContextoAplicacao contextoAplicacao)
        {
            this.contextoAplicacao = contextoAplicacao ?? throw new ArgumentNullException(nameof(contextoAplicacao));
        }
        public Task<Guid> Handle(ObterUsuarioLogadoPerfilAtualQuery request, CancellationToken cancellationToken)
        {
            var perfilAtual = contextoAplicacao.ObterVarivel<IEnumerable<InternalClaim>>("Claims").FirstOrDefault(a => a.Type == CLAIM_PERFIL_ATUAL);
            if (perfilAtual == null)
                throw new NegocioException("Não foi possível obter o perfil atual.");

            return Task.FromResult(Guid.Parse(perfilAtual.Value));
            
        }
    }
}

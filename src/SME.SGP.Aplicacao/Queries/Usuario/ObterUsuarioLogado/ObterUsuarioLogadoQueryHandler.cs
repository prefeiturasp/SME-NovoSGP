using MediatR;
using Newtonsoft.Json;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Contexto;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUsuarioLogadoQueryHandler : IRequestHandler<ObterUsuarioLogadoQuery, Usuario>
    {
        private readonly IServicoUsuario servicoUsuario;

        public ObterUsuarioLogadoQueryHandler(IServicoUsuario servicoUsuario)
        {
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
        }

        public Task<Usuario> Handle(ObterUsuarioLogadoQuery request, CancellationToken cancellationToken)
            => servicoUsuario.ObterUsuarioLogado();
    }
}

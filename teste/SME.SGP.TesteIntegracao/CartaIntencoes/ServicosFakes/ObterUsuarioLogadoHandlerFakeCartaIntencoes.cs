using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.CartaIntencoes.ServicosFakes
{
    internal class ObterUsuarioLogadoHandlerFakeCartaIntencoes : IRequestHandler<ObterUsuarioLogadoQuery, Usuario>
    {
        public ObterUsuarioLogadoHandlerFakeCartaIntencoes()
        { }

        public async Task<Usuario> Handle(ObterUsuarioLogadoQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(new Usuario { Login = "9999999", CodigoRf = "9999999", PerfilAtual = Perfis.PERFIL_PROFESSOR_INFANTIL });
        }
    }
}

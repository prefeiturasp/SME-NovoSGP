using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Aplicacao
{
    public class ObterUsuarioPorCodigoRfLoginOuAdicionaQueryHandler : IRequestHandler<ObterUsuarioPorCodigoRfLoginOuAdicionaQuery,Usuario>
    {
        private readonly IServicoUsuario repositorioUsuario;

        public ObterUsuarioPorCodigoRfLoginOuAdicionaQueryHandler(IServicoUsuario usuario)
        {
            repositorioUsuario = usuario ?? throw new ArgumentNullException(nameof(usuario));
        }

        public async Task<Usuario> Handle(ObterUsuarioPorCodigoRfLoginOuAdicionaQuery request, CancellationToken cancellationToken)
        {
            return await repositorioUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(request.CodigoRf,request.Login,request.Nome,request.Email,request.BuscaLogin);
        }
    }
}
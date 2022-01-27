using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUsuarioPossuiAbrangenciaAcessoSondagemQueryHandler : IRequestHandler<ObterUsuarioPossuiAbrangenciaAcessoSondagemQuery, bool>
    {
        private readonly IRepositorioAbrangencia repositorioAbrangencia;

        public ObterUsuarioPossuiAbrangenciaAcessoSondagemQueryHandler(IRepositorioAbrangencia repositorioAbrangencia)
        {
            this.repositorioAbrangencia = repositorioAbrangencia ?? throw new ArgumentNullException(nameof(repositorioAbrangencia));
        }
        public async  Task<bool> Handle(ObterUsuarioPossuiAbrangenciaAcessoSondagemQuery request, CancellationToken cancellationToken)
        {
            return await repositorioAbrangencia.ObterUsuarioPossuiAbrangenciaAcessoSondagemTiposEscola(request.UsuarioRF, request.UsuarioPerfil);
        }
    }
}

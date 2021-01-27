using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class VerificaUsuarioPossuiArquivoQueryHandler : IRequestHandler<VerificaUsuarioPossuiArquivoQuery, bool>
    {
        private readonly IRepositorioDocumento repositorioDocumento;

        public VerificaUsuarioPossuiArquivoQueryHandler(IRepositorioDocumento repositorioDocumento)
        {
            this.repositorioDocumento = repositorioDocumento ?? throw new ArgumentNullException(nameof(repositorioDocumento));
        }

        public async Task<bool> Handle(VerificaUsuarioPossuiArquivoQuery request, CancellationToken cancellationToken)
            => await repositorioDocumento.ValidarUsuarioPossuiDocumento(request.TipoDocumentoId, request.ClassificacaoId, request.UsuarioId, request.UeId, request.DocumentoId);
    }
}

using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.Armazenamento.ExcluirArquivoRepositorioPorId
{
    public class ExcluirDocumentoPorIdCommandHandler : IRequestHandler<ExcluirDocumentoPorIdCommand, bool>
    {
        private readonly IRepositorioDocumento repositorioDocumento;

        public ExcluirDocumentoPorIdCommandHandler(IRepositorioDocumento repositorioDocumento)
        {
            this.repositorioDocumento = repositorioDocumento ?? throw new ArgumentNullException(nameof(repositorioDocumento));
        }

        public async Task<bool> Handle(ExcluirDocumentoPorIdCommand request, CancellationToken cancellationToken)
            => await repositorioDocumento.ExcluirDocumentoPorId(request.Id);
    }
}

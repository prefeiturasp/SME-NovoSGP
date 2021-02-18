using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.Armazenamento.ExcluirArquivoRepositorioPorId
{
    public class ExcluirArquivoRepositorioPorIdCommandHandler : IRequestHandler<ExcluirArquivoRepositorioPorIdCommand, bool>
    {
        private readonly IRepositorioArquivo repositorioArquivo;

        public ExcluirArquivoRepositorioPorIdCommandHandler(IRepositorioArquivo repositorioArquivo)
        {
            this.repositorioArquivo = repositorioArquivo ?? throw new ArgumentNullException(nameof(repositorioArquivo));
        }

        public async Task<bool> Handle(ExcluirArquivoRepositorioPorIdCommand request, CancellationToken cancellationToken)
            => await repositorioArquivo.ExcluirArquivoPorId(request.Id);
    }
}

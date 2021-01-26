using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirArquivoRepositorioPorCodigoCommandHandler : IRequestHandler<ExcluirArquivoRepositorioPorCodigoCommand, bool>
    {
        private readonly IRepositorioArquivo repositorioArquivo;

        public ExcluirArquivoRepositorioPorCodigoCommandHandler(IRepositorioArquivo repositorioArquivo)
        {
            this.repositorioArquivo = repositorioArquivo ?? throw new ArgumentNullException(nameof(repositorioArquivo));
        }

        public async Task<bool> Handle(ExcluirArquivoRepositorioPorCodigoCommand request, CancellationToken cancellationToken)
            => await repositorioArquivo.ExcluirArquivoPorCodigo(request.CodigoArquivo);
    }
}

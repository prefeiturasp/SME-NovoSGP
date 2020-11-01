using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarArquivoRepositorioCommandHandler : IRequestHandler<SalvarArquivoRepositorioCommand, ArquivoArmazenadoDto>
    {
        private readonly IRepositorioArquivo repositorioArquivo;

        public SalvarArquivoRepositorioCommandHandler(IRepositorioArquivo repositorioArquivo)
        {
            this.repositorioArquivo = repositorioArquivo ?? throw new ArgumentNullException(nameof(repositorioArquivo));
        }

        public async Task<ArquivoArmazenadoDto> Handle(SalvarArquivoRepositorioCommand request, CancellationToken cancellationToken)
        {
            var arquivo = new Arquivo()
            {
                Nome = request.NomeArquivo,
                Codigo = Guid.NewGuid(),
                Tipo = request.Tipo,
                TipoConteudo = request.TipoConteudo
            };

            await repositorioArquivo.SalvarAsync(arquivo);

            return new ArquivoArmazenadoDto(arquivo.Id, arquivo.Codigo);
        }
    }
}

using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Infra.Interface;

namespace SME.SGP.Aplicacao
{
    public class CopiarArquivoCommandHandler : IRequestHandler<CopiarArquivoCommand, string>
    {
        private readonly IRepositorioArquivo repositorioArquivo;

        private readonly IServicoArmazenamento servicoArmazenamento;
        
        private readonly IMediator mediator;
        
        public CopiarArquivoCommandHandler(IRepositorioArquivo repositorioArquivo,IServicoArmazenamento servicoArmazenamento)
        {
            this.repositorioArquivo = repositorioArquivo ?? throw new ArgumentNullException(nameof(repositorioArquivo));
            this.servicoArmazenamento = servicoArmazenamento ?? throw new ArgumentNullException(nameof(servicoArmazenamento));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<string> Handle(CopiarArquivoCommand request, CancellationToken cancellationToken)
        {
            var retorno = await servicoArmazenamento.Copiar(request.Nome);
                
            await SalvarCopiaArquivo(request.TipoArquivoDestino, request.Nome);
            
            await mediator.Send(new OtimizarArquivosCommand(request.Nome));
            
            return retorno;
        }
        private async Task SalvarCopiaArquivo(TipoArquivo tipo, string nomeArquivo)
        {
            var arquivo = await repositorioArquivo.ObterPorCodigo(new Guid(nomeArquivo.Split('.').FirstOrDefault()));
            if (arquivo != null)
            {
                arquivo.Tipo = tipo;
                await repositorioArquivo.SalvarAsync(MapearParaEntidade(tipo, arquivo));
            }
        }
        private Arquivo MapearParaEntidade(TipoArquivo tipo, Arquivo arquivo)
        {
            return new Arquivo
            {
                Nome = arquivo.Nome,
                Codigo = arquivo.Codigo,
                Tipo = tipo,
                TipoConteudo = arquivo.TipoConteudo
            };
        }
    }
}

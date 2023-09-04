using MediatR;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using SME.SGP.Dominio.Constantes;
using SME.SGP.Infra.Utilitarios;

namespace SME.SGP.Aplicacao
{
    public class MoverArquivosTemporariosCommandHandler : IRequestHandler<MoverArquivosTemporariosCommand, string>
    {
        private readonly IMediator mediator;
        private readonly ConfiguracaoArmazenamentoOptions configuracaoArmazenamentoOptions;

        public MoverArquivosTemporariosCommandHandler(IMediator mediator,IOptions<ConfiguracaoArmazenamentoOptions> configuracaoArmazenamentoOptions)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.configuracaoArmazenamentoOptions = configuracaoArmazenamentoOptions?.Value ?? throw new ArgumentNullException(nameof(configuracaoArmazenamentoOptions));
        }
        public async Task<string> Handle(MoverArquivosTemporariosCommand request, CancellationToken cancellationToken)
        {
            var regex = new Regex(ArmazenamentoObjetos.EXPRESSAO_NOME_ARQUIVO);
            var regexImagensPasta = new Regex(ArmazenamentoObjetos.EXPRESSAO_NOME_ARQUIVO_COM_PASTA);

            var novo = regex.Matches(request.TextoEditorNovo).Cast<Match>().Select(c => c.Value).ToList();
            var atual = regex.Matches(!string.IsNullOrEmpty(request.TextoEditorAtual) ? request.TextoEditorAtual : string.Empty).Cast<Match>().Select(c => c.Value).ToList();

            var imagensEditorNovo = regexImagensPasta.Matches(request.TextoEditorNovo).Cast<Match>().Select(c => c.Value).ToList();

            novo = imagensEditorNovo.Any() ? RetornaImagensTemporariasParaMover(imagensEditorNovo) : novo;
            var diferenca = novo.Any() ? novo.Except(atual) : Enumerable.Empty<string>();

            foreach (var item in diferenca)
            {
                await mediator.Send(new MoverArquivoCommand(item, request.TipoArquivo));
                request.TextoEditorNovo = request.TextoEditorNovo.Replace(configuracaoArmazenamentoOptions.BucketTemp, configuracaoArmazenamentoOptions.BucketArquivos);
            }
            
            return request.TextoEditorNovo;
        }

        public List<string> RetornaImagensTemporariasParaMover(List<string> novosArquivosTextoNovo)
        {
            var imagensParaMover = new List<string>();

            foreach(var arquivo in novosArquivosTextoNovo)
            {
                bool jaExisteImagem = arquivo.Contains($@"/{configuracaoArmazenamentoOptions.BucketArquivos}/");

                if (!jaExisteImagem)
                {
                    var separaArquivoParaMover = arquivo.Split('/');

                    if(separaArquivoParaMover.Length == 3)
                        imagensParaMover.Add(separaArquivoParaMover[2]);
                }                  
            }

            return imagensParaMover;
        }
    }
}

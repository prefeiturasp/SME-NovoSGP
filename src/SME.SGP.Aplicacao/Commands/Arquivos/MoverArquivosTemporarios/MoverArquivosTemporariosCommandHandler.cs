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
            var novo = regex.Matches(request.TextoEditorNovo).Cast<Match>().Select(c => c.Value).ToList();
            var imagensNovas = Regex.Matches(request.TextoEditorNovo, "<img[^>]*>");

            novo = imagensNovas.Any() ? VerificaSeTextoNovoContemImagensExistentes(novo, imagensNovas) : novo;

            var atual = regex.Matches(!string.IsNullOrEmpty(request.TextoEditorAtual)?request.TextoEditorAtual:string.Empty).Cast<Match>().Select(c => c.Value).ToList();
            var diferenca = novo.Any() ? novo.Except(atual) : new  List<string>();

            foreach (var item in diferenca)
            {
                await mediator.Send(new MoverArquivoCommand(item, request.TipoArquivo));
                request.TextoEditorNovo = request.TextoEditorNovo.Replace(configuracaoArmazenamentoOptions.BucketTemp, configuracaoArmazenamentoOptions.BucketArquivos);
            }
            
            return request.TextoEditorNovo;
        }

        public List<string> VerificaSeTextoNovoContemImagensExistentes(List<string> novosArquivosTextoNovo, MatchCollection imagensNovas)
        {
            var imagensParaMover = new List<string>();

            foreach(var arquivo in novosArquivosTextoNovo)
            {
                bool jaExisteImagem = imagensNovas.Any(i => i.Value.Contains(arquivo));

                if (!jaExisteImagem)
                    imagensParaMover.Add(arquivo);
            }

            return imagensParaMover;
        }
    }
}

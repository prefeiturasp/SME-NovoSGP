using System;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Infra.Interface;
using SME.SGP.Infra.Utilitarios;

namespace SME.SGP.Aplicacao
{
    public class RemoverArquivosExcluidosCommandHandler : IRequestHandler<RemoverArquivosExcluidosCommand, bool>
    {
        private readonly IServicoArmazenamento servicoArmazenamento;
        private readonly ConfiguracaoArmazenamentoOptions configuracaoArmazenamentoOptions;

        public RemoverArquivosExcluidosCommandHandler(IMediator mediator,IServicoArmazenamento servicoArmazenamento, ConfiguracaoArmazenamentoOptions configuracaoArmazenamentoOptions)  
        {
            this.servicoArmazenamento = servicoArmazenamento ?? throw new ArgumentNullException(nameof(servicoArmazenamento));
            this.configuracaoArmazenamentoOptions = configuracaoArmazenamentoOptions ?? throw new ArgumentNullException(nameof(configuracaoArmazenamentoOptions));
        }
        
        public async Task<bool> Handle(RemoverArquivosExcluidosCommand request, CancellationToken cancellationToken)
        {
            var arquivoAtual = request.ArquivoAtual.Replace(@"\", @"/");
            var arquivoNovo = request.ArquivoNovo.Replace(@"\", @"/");
            var expressao = @"\/[0-9]{4}\/[0-9]{2}\/[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}.[A-Za-z0-4]+";
            var regex = new Regex(expressao);
            var atual = regex.Matches(arquivoAtual).Cast<Match>().Select(c => c.Value).ToList();
            var novo = regex.Matches(arquivoNovo).Cast<Match>().Select(c => c.Value).ToList();
            var diferente = atual.Except(novo);
            
            foreach (var item in diferente)
                await servicoArmazenamento.Excluir(item,configuracaoArmazenamentoOptions.BucketSGP);
            
            return true;
        }
    }
}

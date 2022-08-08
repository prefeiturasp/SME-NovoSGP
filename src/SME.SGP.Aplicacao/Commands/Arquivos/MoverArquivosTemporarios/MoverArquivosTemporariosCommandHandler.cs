using MediatR;
using SME.SGP.Dominio;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Collections.Generic;
using SME.SGP.Infra;
using SME.SGP.Infra.Utilitarios;

namespace SME.SGP.Aplicacao
{
    public class MoverArquivosTemporariosCommandHandler : IRequestHandler<MoverArquivosTemporariosCommand, string>
    {
        private readonly IMediator mediator;
        private readonly ConfiguracaoArmazenamentoOptions configuracaoArmazenamentoOptions;

        public MoverArquivosTemporariosCommandHandler(IMediator mediator,ConfiguracaoArmazenamentoOptions configuracaoArmazenamentoOptions)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.configuracaoArmazenamentoOptions = configuracaoArmazenamentoOptions ?? throw new ArgumentNullException(nameof(configuracaoArmazenamentoOptions));
        }
        public async Task<string> Handle(MoverArquivosTemporariosCommand request, CancellationToken cancellationToken)
        {
            var enderecoFuncionalidade = string.Empty;
            var expressao = @"[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}.[A-Za-z0-4]+";
            var regex = new Regex(expressao);
            var novo = regex.Matches(request.TextoEditorNovo).Cast<Match>().Select(c => c.Value).ToList();
            var atual = regex.Matches(!string.IsNullOrEmpty(request.TextoEditorAtual)?request.TextoEditorAtual:string.Empty).Cast<Match>().Select(c => c.Value).ToList();
            var diferenca = novo.Any() ? novo.Except(atual) : new  List<string>();

            foreach (var item in diferenca)
            {
                await mediator.Send(new MoverArquivoCommand(item, request.TipoArquivo));
                request.TextoEditorNovo = request.TextoEditorNovo.Replace(configuracaoArmazenamentoOptions.BucketTempSGPName, configuracaoArmazenamentoOptions.BucketSGP);
            }

            return request.TextoEditorNovo;
        }
    }
}

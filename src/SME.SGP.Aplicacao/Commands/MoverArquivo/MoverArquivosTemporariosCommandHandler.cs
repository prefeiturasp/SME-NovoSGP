using MediatR;
using SME.SGP.Dominio;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class MoverArquivosTemporariosCommandHandler : IRequestHandler<MoverArquivosTemporariosCommand, string>
    {
        private readonly IMediator mediator;
        public MoverArquivosTemporariosCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
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
                enderecoFuncionalidade = await mediator.Send(new MoverArquivoCommand(item, request.TipoArquivo));
            }

            return request.TextoEditorNovo.Replace("/Temp/", enderecoFuncionalidade);

        }
    }
}

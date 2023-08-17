using MediatR;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SME.SGP.Infra.Interface;
using SME.SGP.Infra.Utilitarios;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao.Commands.DeletarArquivo
{
    public class DeletarArquivoDeRegistroExcluidoCommandHandler : IRequestHandler<DeletarArquivoDeRegistroExcluidoCommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IServicoArmazenamento servicoArmazenamento;
        private readonly ConfiguracaoArmazenamentoOptions configuracaoArmazenamentoOptions;

        public DeletarArquivoDeRegistroExcluidoCommandHandler(IMediator mediator, IServicoArmazenamento servicoArmazenamento, IOptions<ConfiguracaoArmazenamentoOptions> configuracaoArmazenamentoOptions)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.servicoArmazenamento = servicoArmazenamento ?? throw new ArgumentNullException(nameof(servicoArmazenamento));
            this.configuracaoArmazenamentoOptions = configuracaoArmazenamentoOptions?.Value ?? throw new ArgumentNullException(nameof(configuracaoArmazenamentoOptions));
        }

        public async Task<bool> Handle(DeletarArquivoDeRegistroExcluidoCommand request, CancellationToken cancellationToken)
        {
            var arquivoAtual = request.ArquivoAtual.Replace(@"\", @"/");
            var atual = UtilRegex.RegexNomesArquivosUUID.Matches(arquivoAtual).Cast<Match>().Select(c => c.Value).ToList();

            return await DeletarArquivo(atual);

        }

        private async Task<bool> DeletarArquivo(IEnumerable arquivos)
        {
            var retornosExclusao = new List<bool>();
            foreach (var item in arquivos)
            {
                retornosExclusao.Add(await servicoArmazenamento.Excluir(item.ToString()));;
            }

            return retornosExclusao.All(a => a != false);
        }
    }
}
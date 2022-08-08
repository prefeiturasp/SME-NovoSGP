using MediatR;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Infra.Interface;
using SME.SGP.Infra.Utilitarios;

namespace SME.SGP.Aplicacao.Commands.DeletarArquivo
{
    public class DeletarArquivoDeRegistroExcluidoCommandHandler : IRequestHandler<DeletarArquivoDeRegistroExcluidoCommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IServicoArmazenamento servicoArmazenamento;
        private readonly ConfiguracaoArmazenamentoOptions configuracaoArmazenamentoOptions;

        public DeletarArquivoDeRegistroExcluidoCommandHandler(IMediator mediator,IServicoArmazenamento servicoArmazenamento,ConfiguracaoArmazenamentoOptions configuracaoArmazenamentoOptions)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.servicoArmazenamento = servicoArmazenamento ?? throw new ArgumentNullException(nameof(servicoArmazenamento));
            this.configuracaoArmazenamentoOptions = configuracaoArmazenamentoOptions ?? throw new ArgumentNullException(nameof(configuracaoArmazenamentoOptions));
        }

        public async Task<bool> Handle(DeletarArquivoDeRegistroExcluidoCommand request, CancellationToken cancellationToken)
        {
            var arquivoAtual = request.ArquivoAtual.Replace(@"\", @"/");
            
            var expressao = @"\/[0-9]{4}\/[0-9]{2}\/[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}.[A-Za-z0-4]+";
            
            var regex = new Regex(expressao);
            
            var atual = regex.Matches(arquivoAtual).Cast<Match>().Select(c => c.Value).ToList();
            
            await DeletarArquivo(atual);
            
            return true;
        }

        private async Task DeletarArquivo(IEnumerable arquivos)
        {
            foreach (var item in arquivos)
            {
                try
                {
                    await servicoArmazenamento.Excluir(item.ToString(),configuracaoArmazenamentoOptions.BucketSGP);
                }
                catch (Exception ex)
                {
                    await mediator.Send(new SalvarLogViaRabbitCommand($"Falha ao deletar o arquivo {ex.Message}",
                                                                      LogNivel.Critico,
                                                                      LogContexto.Arquivos));
                }
            }

        }
    }
}

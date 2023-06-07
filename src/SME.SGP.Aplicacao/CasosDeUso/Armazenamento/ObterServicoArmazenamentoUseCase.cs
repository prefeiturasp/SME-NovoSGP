using System;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Infra.Interface;

namespace SME.SGP.Aplicacao
{
    public class ObterServicoArmazenamentoUseCase : AbstractUseCase, IObterServicoArmazenamentoUseCase
    {
        private readonly IServicoArmazenamento servicoArmazenamento;

        public ObterServicoArmazenamentoUseCase(IServicoArmazenamento servicoArmazenamento,IMediator mediator) : base(mediator)
        {
            this.servicoArmazenamento = servicoArmazenamento ?? throw new ArgumentNullException(nameof(servicoArmazenamento));
        } 

        public async Task<string> Executar(string nomeArquivo, bool ehPastaTemporaria)
        {
            var retorno = await servicoArmazenamento.Obter(nomeArquivo,ehPastaTemporaria);
            
            return retorno;
        }
    }
}

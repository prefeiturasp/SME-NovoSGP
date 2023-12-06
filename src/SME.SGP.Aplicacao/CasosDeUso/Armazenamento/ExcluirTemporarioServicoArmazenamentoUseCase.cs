using System;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Infra.Interface;

namespace SME.SGP.Aplicacao
{
    public class ExcluirTemporarioServicoArmazenamentoUseCase : AbstractUseCase, IExcluirTemporarioServicoArmazenamentoUseCase
    {
        private readonly IServicoArmazenamento servicoArmazenamento;

        public ExcluirTemporarioServicoArmazenamentoUseCase(IServicoArmazenamento servicoArmazenamento,IMediator mediator) : base(mediator)
        {
            this.servicoArmazenamento = servicoArmazenamento ?? throw new ArgumentNullException(nameof(servicoArmazenamento));
        } 

        public async Task<bool> Executar(string nomeArquivo, string bucketTemporario)
        {
            var retorno = await servicoArmazenamento.Excluir(nomeArquivo, bucketTemporario);
            
            return retorno;
        }
    }
}

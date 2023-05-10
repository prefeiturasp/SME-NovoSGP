using System;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Infra.Interface;

namespace SME.SGP.Aplicacao
{
    public class MoverServicoArmazenamentoUseCase : AbstractUseCase, IMoverServicoArmazenamentoUseCase
    {
        private readonly IServicoArmazenamento servicoArmazenamento;

        public MoverServicoArmazenamentoUseCase(IServicoArmazenamento servicoArmazenamento,IMediator mediator) : base(mediator)
        {
            this.servicoArmazenamento = servicoArmazenamento ?? throw new ArgumentNullException(nameof(servicoArmazenamento));
        } 

        public async Task<string> Executar(string nomeArquivo)
        {
            var retorno = await servicoArmazenamento.Mover(nomeArquivo);
            
            return retorno;
        }
    }
}

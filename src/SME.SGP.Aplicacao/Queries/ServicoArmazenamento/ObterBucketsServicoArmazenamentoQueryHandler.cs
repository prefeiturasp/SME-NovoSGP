using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Infra.Interface;

namespace SME.SGP.Aplicacao
{
    public class ObterBucketsServicoArmazenamentoQueryHandler: IRequestHandler<ObterBucketsServicoArmazenamentoQuery, bool>
    {
        private readonly IServicoArmazenamento servicoArmazenamento;
        
        public ObterBucketsServicoArmazenamentoQueryHandler(IServicoArmazenamento servicoArmazenamento)
        {
            this.servicoArmazenamento = servicoArmazenamento ?? throw new ArgumentNullException(nameof(servicoArmazenamento));
        }

        public async Task<bool> Handle(ObterBucketsServicoArmazenamentoQuery request, CancellationToken cancellationToken)
        {
            await servicoArmazenamento.Obter();
            return true;
        }
    }
}
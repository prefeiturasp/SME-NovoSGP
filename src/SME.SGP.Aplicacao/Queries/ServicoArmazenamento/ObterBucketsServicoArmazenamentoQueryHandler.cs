using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Infra.Interface;

namespace SME.SGP.Aplicacao
{
    public class ObterBucketsServicoArmazenamentoQueryHandler: IRequestHandler<ObterBucketsServicoArmazenamentoQuery, IEnumerable<string>>
    {
        private readonly IServicoArmazenamento servicoArmazenamento;
        
        public ObterBucketsServicoArmazenamentoQueryHandler(IServicoArmazenamento servicoArmazenamento)
        {
            this.servicoArmazenamento = servicoArmazenamento ?? throw new ArgumentNullException(nameof(servicoArmazenamento));
        }

        public async Task<IEnumerable<string>> Handle(ObterBucketsServicoArmazenamentoQuery request, CancellationToken cancellationToken)
        {
            return await servicoArmazenamento.ObterBuckets();
        }
    }
}
using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterNotaEmAprovacaoPorFechamentoNotaIdQueryHandler : IRequestHandler<ObterNotaEmAprovacaoPorFechamentoNotaIdQuery, IEnumerable<FechamentoNotaAprovacaoDto>>
    {
        private readonly IRepositorioFechamentoNotaConsulta repositorioFechamentoNotaConsulta;

        public ObterNotaEmAprovacaoPorFechamentoNotaIdQueryHandler(IRepositorioFechamentoNotaConsulta repositorioFechamentoNotaConsulta)
        {
            this.repositorioFechamentoNotaConsulta = repositorioFechamentoNotaConsulta ?? throw new ArgumentNullException(nameof(repositorioFechamentoNotaConsulta));
        }
        public async Task<IEnumerable<FechamentoNotaAprovacaoDto>> Handle(ObterNotaEmAprovacaoPorFechamentoNotaIdQuery request, CancellationToken cancellationToken)
            => await repositorioFechamentoNotaConsulta.ObterNotasEmAprovacaoPorIdsFechamento(request.IdsFechamentoNota);
        
    }
}

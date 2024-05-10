using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirConsolidacaoFechamentoPorTurmaIdEBimestreCommandHandler : AsyncRequestHandler<ExcluirConsolidacaoFechamentoPorTurmaIdEBimestreCommand>
    {
        private readonly IRepositorioFechamentoConsolidado repositorioFechamentoConsolidado;

        public ExcluirConsolidacaoFechamentoPorTurmaIdEBimestreCommandHandler(IRepositorioFechamentoConsolidado repositorioFechamentoConsolidado)
        {
            this.repositorioFechamentoConsolidado = repositorioFechamentoConsolidado ?? throw new ArgumentNullException(nameof(repositorioFechamentoConsolidado));
        }

        protected override async Task Handle(ExcluirConsolidacaoFechamentoPorTurmaIdEBimestreCommand request, CancellationToken cancellationToken)
        {
            await repositorioFechamentoConsolidado.ExcluirConsolidacaoPorTurmaIdEBimestre(request.TurmaId, request.Bimestre);
        }
    }
}

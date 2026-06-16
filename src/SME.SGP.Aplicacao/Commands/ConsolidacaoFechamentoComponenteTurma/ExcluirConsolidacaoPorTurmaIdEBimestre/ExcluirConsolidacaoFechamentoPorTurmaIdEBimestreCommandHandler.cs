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
    public class ExcluirConsolidacaoFechamentoPorTurmaIdEBimestreCommandHandler : IRequestHandler<ExcluirConsolidacaoFechamentoPorTurmaIdEBimestreCommand>
    {
        private readonly IRepositorioFechamentoConsolidado repositorioFechamentoConsolidado;

        public ExcluirConsolidacaoFechamentoPorTurmaIdEBimestreCommandHandler(IRepositorioFechamentoConsolidado repositorioFechamentoConsolidado)
        {
            this.repositorioFechamentoConsolidado = repositorioFechamentoConsolidado ?? throw new ArgumentNullException(nameof(repositorioFechamentoConsolidado));
        }

        public async Task Handle(ExcluirConsolidacaoFechamentoPorTurmaIdEBimestreCommand request, CancellationToken cancellationToken)
        {
            await repositorioFechamentoConsolidado.ExcluirConsolidacaoPorTurmaIdEBimestre(request.TurmaId, request.Bimestre);
        }
    }
}

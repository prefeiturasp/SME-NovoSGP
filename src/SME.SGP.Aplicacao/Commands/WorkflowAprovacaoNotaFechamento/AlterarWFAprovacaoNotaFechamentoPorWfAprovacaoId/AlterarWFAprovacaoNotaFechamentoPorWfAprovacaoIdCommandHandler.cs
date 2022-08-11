using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AlterarWFAprovacaoNotaFechamentoPorWfAprovacaoIdCommandHandler : IRequestHandler<AlterarWFAprovacaoNotaFechamentoPorWfAprovacaoIdCommand, bool>
    {
        private readonly IRepositorioWfAprovacaoNotaFechamento repositorioWfAprovacaoNotaFechamento;

        public AlterarWFAprovacaoNotaFechamentoPorWfAprovacaoIdCommandHandler(IRepositorioWfAprovacaoNotaFechamento repositorioWfAprovacaoNotaFechamento)
        {
            this.repositorioWfAprovacaoNotaFechamento = repositorioWfAprovacaoNotaFechamento ?? throw new ArgumentNullException(nameof(repositorioWfAprovacaoNotaFechamento));
        }
        public Task<bool> Handle(AlterarWFAprovacaoNotaFechamentoPorWfAprovacaoIdCommand request, CancellationToken cancellationToken)
         => repositorioWfAprovacaoNotaFechamento.AlterarWfAprovacaoNotaFechamentoComWfAprovacaoId(request.WorkflowAprovacaoId, request.WorkflowAprovacaoFechamentoNotaIds);
    }
}

using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirWFAprovacaoNotaFechamentoCommandHandler : AsyncRequestHandler<ExcluirWFAprovacaoNotaFechamentoCommand>
    {
        private readonly IRepositorioWfAprovacaoNotaFechamento repositorio;

        public ExcluirWFAprovacaoNotaFechamentoCommandHandler(IRepositorioWfAprovacaoNotaFechamento repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        protected override async Task Handle(ExcluirWFAprovacaoNotaFechamentoCommand request, CancellationToken cancellationToken)
            => await repositorio.Excluir(request.WfAprovacaoNotaFechamento);
    }
}

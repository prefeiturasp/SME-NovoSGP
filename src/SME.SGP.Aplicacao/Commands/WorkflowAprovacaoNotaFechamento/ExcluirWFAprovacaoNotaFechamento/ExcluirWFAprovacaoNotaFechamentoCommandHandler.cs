using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirWFAprovacaoNotaFechamentoCommandHandler : IRequestHandler<ExcluirWFAprovacaoNotaFechamentoCommand>
    {
        private readonly IRepositorioWfAprovacaoNotaFechamento repositorio;

        public ExcluirWFAprovacaoNotaFechamentoCommandHandler(IRepositorioWfAprovacaoNotaFechamento repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task Handle(ExcluirWFAprovacaoNotaFechamentoCommand request, CancellationToken cancellationToken)
            => await repositorio.ExcluirLogico(request.WfAprovacaoNotaFechamento);
    }
}

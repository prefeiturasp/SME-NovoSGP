using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ExcluirConsolidadoAtendimentoNAAPAPorIdCommandHandler : IRequestHandler<ExcluirConsolidadoAtendimentoNAAPAPorIdCommand, bool>
    {
        private readonly IRepositorioConsolidadoEncaminhamentoNAAPA repositorio;

        public ExcluirConsolidadoAtendimentoNAAPAPorIdCommandHandler(IRepositorioConsolidadoEncaminhamentoNAAPA repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public Task<bool> Handle(ExcluirConsolidadoAtendimentoNAAPAPorIdCommand request, CancellationToken cancellationToken)
        {
            repositorio.Remover(request.Id);
            return Task.Run(() => true);
        }
    }
}
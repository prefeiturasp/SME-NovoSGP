using MediatR;
using SME.SGP.Dominio;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirConsolidadoEncaminhamentoNAAPAPorIdCommandHandler : IRequestHandler<ExcluirConsolidadoEncaminhamentoNAAPAPorIdCommand, bool>
    {
        private readonly IRepositorioConsolidadoEncaminhamentoNAAPA repositorio;

        public ExcluirConsolidadoEncaminhamentoNAAPAPorIdCommandHandler(IRepositorioConsolidadoEncaminhamentoNAAPA repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public Task<bool> Handle(ExcluirConsolidadoEncaminhamentoNAAPAPorIdCommand request, CancellationToken cancellationToken)
        {
            repositorio.Remover(request.Id);
            return Task.Run(() => true);
        }
    }
}
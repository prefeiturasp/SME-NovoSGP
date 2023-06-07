using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ExcluirConsolidadoAtendimentoProfissionalEncaminhamentoNAAPAPorIdCommandHandler : IRequestHandler<ExcluirConsolidadoAtendimentoProfissionalEncaminhamentoNAAPAPorIdCommand, bool>
    {
        private readonly IRepositorioConsolidadoAtendimentoNAAPA repositorio;

        public ExcluirConsolidadoAtendimentoProfissionalEncaminhamentoNAAPAPorIdCommandHandler(IRepositorioConsolidadoAtendimentoNAAPA repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public Task<bool> Handle(ExcluirConsolidadoAtendimentoProfissionalEncaminhamentoNAAPAPorIdCommand request, CancellationToken cancellationToken)
        {
            repositorio.Remover(request.Id);
            return Task.Run(() => true);
        }
    }
}
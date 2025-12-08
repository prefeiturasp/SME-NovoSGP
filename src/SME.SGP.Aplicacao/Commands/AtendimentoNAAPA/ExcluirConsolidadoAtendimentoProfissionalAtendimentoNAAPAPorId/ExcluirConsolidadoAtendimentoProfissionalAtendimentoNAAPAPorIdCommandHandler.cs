using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ExcluirConsolidadoAtendimentoProfissionalAtendimentoNAAPAPorIdCommandHandler : IRequestHandler<ExcluirConsolidadoAtendimentoProfissionalAtendimentoNAAPAPorIdCommand, bool>
    {
        private readonly IRepositorioConsolidadoAtendimentoNAAPA repositorio;

        public ExcluirConsolidadoAtendimentoProfissionalAtendimentoNAAPAPorIdCommandHandler(IRepositorioConsolidadoAtendimentoNAAPA repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public Task<bool> Handle(ExcluirConsolidadoAtendimentoProfissionalAtendimentoNAAPAPorIdCommand request, CancellationToken cancellationToken)
        {
            repositorio.Remover(request.Id);
            return Task.Run(() => true);
        }
    }
}
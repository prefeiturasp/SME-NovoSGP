using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirNotificacaoTotalPorIdCommandHandler : IRequestHandler<ExcluirNotificacaoTotalPorIdCommand, bool>
    {
        private readonly IRepositorioNotificacao repositorio;
        
        public ExcluirNotificacaoTotalPorIdCommandHandler(IRepositorioNotificacao repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public Task<bool> Handle(ExcluirNotificacaoTotalPorIdCommand request, CancellationToken cancellationToken)
        {
            return repositorio.ExcluirTotalPorIdsAsync(request.NotificacaoId);
        }
    }
}

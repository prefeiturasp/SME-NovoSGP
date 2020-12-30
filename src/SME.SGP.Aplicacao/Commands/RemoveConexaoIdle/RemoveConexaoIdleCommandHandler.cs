using MediatR;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RemoveConexaoIdleCommandHandler : IRequestHandler<RemoveConexaoIdleCommand, bool>
    {
        private readonly IRepositorioRemoveConexaoIdle repositorioRemoveConexaoIdle;

        public RemoveConexaoIdleCommandHandler(IRepositorioRemoveConexaoIdle repositorioRemoveConexaoIdle)
        {
            this.repositorioRemoveConexaoIdle = repositorioRemoveConexaoIdle ?? throw new System.ArgumentNullException(nameof(repositorioRemoveConexaoIdle));
        }
        public Task<bool> Handle(RemoveConexaoIdleCommand request, CancellationToken cancellationToken)
        {
            repositorioRemoveConexaoIdle.RemoveConexaoIdle();
            return Task.FromResult(true);
        }
    }
}

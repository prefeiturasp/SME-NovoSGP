using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarCadastroAcessoABAECommandHandler : IRequestHandler<SalvarCadastroAcessoABAECommand, long>
    {
        private readonly IRepositorioCadastroAcessoABAE repositorioCadastroAcessoABAE;

        public SalvarCadastroAcessoABAECommandHandler(IRepositorioCadastroAcessoABAE repositorioCadastroAcessoABA)
        {
            this.repositorioCadastroAcessoABAE = repositorioCadastroAcessoABA ?? throw new ArgumentNullException(nameof(repositorioCadastroAcessoABA));
        }

        public async Task<long> Handle(SalvarCadastroAcessoABAECommand request, CancellationToken cancellationToken)
        {
            return await repositorioCadastroAcessoABAE.SalvarAsync(request.CadastroAcessoABAE);
        }
    }
}

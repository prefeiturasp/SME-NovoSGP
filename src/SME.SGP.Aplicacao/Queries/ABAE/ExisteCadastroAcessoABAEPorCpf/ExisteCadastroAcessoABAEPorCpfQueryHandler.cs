using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ExisteCadastroAcessoABAEPorCpfQueryHandler : IRequestHandler<ExisteCadastroAcessoABAEPorCpfQuery, bool>
    {
        private readonly IRepositorioCadastroAcessoABAEConsulta repositorioCadastroAcessoABAEConsulta;
        
        public ExisteCadastroAcessoABAEPorCpfQueryHandler(IRepositorioCadastroAcessoABAEConsulta repositorioCadastroAcessoABAEConsulta)
        {
            this.repositorioCadastroAcessoABAEConsulta = repositorioCadastroAcessoABAEConsulta ?? throw new ArgumentNullException(nameof(repositorioCadastroAcessoABAEConsulta));
        }
        
        public async Task<bool> Handle(ExisteCadastroAcessoABAEPorCpfQuery request, CancellationToken cancellationToken)
        {
            return await repositorioCadastroAcessoABAEConsulta.ExisteCadastroAcessoABAEPorCpf(request.Cpf, request.UeId);
        }
    }
}

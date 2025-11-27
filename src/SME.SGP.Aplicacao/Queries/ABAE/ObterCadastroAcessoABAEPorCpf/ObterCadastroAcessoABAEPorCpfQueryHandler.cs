using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterCadastroAcessoABAEPorCpfUsuarioQueryHandler : IRequestHandler<ObterCadastroAcessoABAEPorCpfQuery, CadastroAcessoABAE>
    {
        private readonly IRepositorioCadastroAcessoABAEConsulta repositorioCadastroAcessoABAEConsulta;
        
        public ObterCadastroAcessoABAEPorCpfUsuarioQueryHandler(IRepositorioCadastroAcessoABAEConsulta repositorioCadastroAcessoABAEConsulta)
        {
            this.repositorioCadastroAcessoABAEConsulta = repositorioCadastroAcessoABAEConsulta ?? throw new ArgumentNullException(nameof(repositorioCadastroAcessoABAEConsulta));
        }
        
        public Task<CadastroAcessoABAE> Handle(ObterCadastroAcessoABAEPorCpfQuery request, CancellationToken cancellationToken)
        {
            return repositorioCadastroAcessoABAEConsulta.ObterCadastroABAEPorCpf(request.Cpf);
        }
    }
}

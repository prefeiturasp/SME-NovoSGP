using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterCadastroAcessoABAEPorIdQueryHandler : IRequestHandler<ObterCadastroAcessoABAEPorIdQuery, CadastroAcessoABAE>
    {
        private readonly IRepositorioCadastroAcessoABAEConsulta repositorioCadastroAcessoABAEConsulta;
        
        public ObterCadastroAcessoABAEPorIdQueryHandler(IRepositorioCadastroAcessoABAEConsulta repositorioCadastroAcessoABAEConsulta)
        {
            this.repositorioCadastroAcessoABAEConsulta = repositorioCadastroAcessoABAEConsulta ?? throw new ArgumentNullException(nameof(repositorioCadastroAcessoABAEConsulta));
        }
        
        public Task<CadastroAcessoABAE> Handle(ObterCadastroAcessoABAEPorIdQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(repositorioCadastroAcessoABAEConsulta.ObterPorId(request.Id));
        }
    }
}

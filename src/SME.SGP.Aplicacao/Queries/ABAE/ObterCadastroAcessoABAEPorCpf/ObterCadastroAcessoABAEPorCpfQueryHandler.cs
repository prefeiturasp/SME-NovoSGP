using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterCadastroAcessoABAEPorCpfUsuarioQueryHandler : IRequestHandler<ObterCadastroAcessoABAEPorCpfQuery, IEnumerable<CadastroAcessoABAE>>
    {
        private readonly IRepositorioCadastroAcessoABAEConsulta repositorioCadastroAcessoABAEConsulta;

        public ObterCadastroAcessoABAEPorCpfUsuarioQueryHandler(IRepositorioCadastroAcessoABAEConsulta repositorioCadastroAcessoABAEConsulta)
        {
            this.repositorioCadastroAcessoABAEConsulta = repositorioCadastroAcessoABAEConsulta ?? throw new ArgumentNullException(nameof(repositorioCadastroAcessoABAEConsulta));
        }

        public Task<IEnumerable<CadastroAcessoABAE>> Handle(ObterCadastroAcessoABAEPorCpfQuery request, CancellationToken cancellationToken)
        {
            return repositorioCadastroAcessoABAEConsulta.ObterCadastrosABAEPorCpf(request.Cpf);
        }
    }
}

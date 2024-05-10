using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterCadastroAcessoABAEPorDreQueryHandler : IRequestHandler<ObterCadastroAcessoABAEPorDreQuery, IEnumerable<NomeCpfABAEDto>>
    {
        private readonly IRepositorioCadastroAcessoABAEConsulta repositorio;

        public ObterCadastroAcessoABAEPorDreQueryHandler(IRepositorioCadastroAcessoABAEConsulta repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public Task<IEnumerable<NomeCpfABAEDto>> Handle(ObterCadastroAcessoABAEPorDreQuery request, CancellationToken cancellationToken)
        {
            return repositorio.ObterCadastrosABAEPorDre(request.Cpf, request.CodigoDre, request.CodigoUe, request.Nome);
        }
    }
}

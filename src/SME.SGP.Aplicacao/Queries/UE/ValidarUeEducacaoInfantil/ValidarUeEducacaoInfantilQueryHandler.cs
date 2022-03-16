using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ValidarUeEducacaoInfantilQueryHandler : IRequestHandler<ValidarUeEducacaoInfantilQuery, bool>
    {
        private readonly IRepositorioUeConsulta repositorioUe;

        public ValidarUeEducacaoInfantilQueryHandler(IRepositorioUeConsulta repositorioUe)
        {
            this.repositorioUe = repositorioUe ?? throw new ArgumentNullException(nameof(repositorioUe));
        }

        public async Task<bool> Handle(ValidarUeEducacaoInfantilQuery request, CancellationToken cancellationToken)
            => await repositorioUe.ValidarUeEducacaoInfantil(request.UeId);
    }
}

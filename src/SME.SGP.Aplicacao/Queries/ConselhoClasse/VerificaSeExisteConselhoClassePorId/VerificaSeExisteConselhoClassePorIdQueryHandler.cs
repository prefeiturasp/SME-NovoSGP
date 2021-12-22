
using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class VerificaSeExisteConselhoClassePorIdQueryHandler: IRequestHandler<VerificaSeExisteConselhoClassePorIdQuery, bool>
    {
        private readonly IRepositorioConselhoClasseConsulta repositorioConselhoClasseConsulta;

        public VerificaSeExisteConselhoClassePorIdQueryHandler(IRepositorioConselhoClasseConsulta repositorioConselhoClasseConsulta)
        {
            this.repositorioConselhoClasseConsulta = repositorioConselhoClasseConsulta ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseConsulta));
        }

        public async Task<bool> Handle(VerificaSeExisteConselhoClassePorIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioConselhoClasseConsulta.Exists(request.ConselhoClasseId);
        }

    }
}

using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmaPorConselhoClasseIdQueryHandler : IRequestHandler<ObterTurmaPorConselhoClasseIdQuery, string>
    {
        private readonly IRepositorioTurmaConsulta repositorioTurmaConsulta;

        public ObterTurmaPorConselhoClasseIdQueryHandler(IRepositorioTurmaConsulta repositorioTurmaConsulta)
        {
            this.repositorioTurmaConsulta = repositorioTurmaConsulta ?? throw new ArgumentNullException(nameof(repositorioTurmaConsulta));
        }
        public async Task<string> Handle(ObterTurmaPorConselhoClasseIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioTurmaConsulta.ObterTurmaCodigoPorConselhoClasseId(request.ConselhoClasseId);
        }
    }
}

using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class VerificaSeExisteFechamentoTurmaPorIdQueryHandler : IRequestHandler<VerificaSeExisteFechamentoTurmaPorIdQuery, bool>
    {
        private readonly IRepositorioFechamentoTurmaConsulta repositorioFechamentoTurmaConsulta;

        public VerificaSeExisteFechamentoTurmaPorIdQueryHandler(IRepositorioFechamentoTurmaConsulta repositorioFechamentoTurmaConsulta)
        {
            this.repositorioFechamentoTurmaConsulta = repositorioFechamentoTurmaConsulta ?? throw new ArgumentNullException(nameof(repositorioFechamentoTurmaConsulta));
        }

        public async Task<bool> Handle(VerificaSeExisteFechamentoTurmaPorIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioFechamentoTurmaConsulta.Exists(request.FechamentoTurmaId);
        }

    }
}

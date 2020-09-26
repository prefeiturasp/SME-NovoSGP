using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class VerificaSeExisteFechamentoTurmaPorIdQueryHandler : IRequestHandler<VerificaSeExisteFechamentoTurmaPorIdQuery, bool>
    {
        private readonly IRepositorioFechamentoTurma repositorioFechamentoTurma;

        public VerificaSeExisteFechamentoTurmaPorIdQueryHandler(IRepositorioFechamentoTurma repositorioFechamentoTurma)
        {
            this.repositorioFechamentoTurma = repositorioFechamentoTurma ?? throw new ArgumentNullException(nameof(repositorioFechamentoTurma));
        }

        public async Task<bool> Handle(VerificaSeExisteFechamentoTurmaPorIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioFechamentoTurma.Exists(request.FechamentoTurmaId);
        }

    }
}

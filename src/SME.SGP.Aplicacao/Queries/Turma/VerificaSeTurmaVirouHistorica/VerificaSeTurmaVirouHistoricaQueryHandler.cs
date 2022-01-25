using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class VerificaSeTurmaVirouHistoricaQueryHandler : IRequestHandler<VerificaSeTurmaVirouHistoricaQuery, bool>
    {
        private readonly IRepositorioTurma repositorioTurma;

        public VerificaSeTurmaVirouHistoricaQueryHandler(IRepositorioTurma repositorioTurma)
        {
            this.repositorioTurma = repositorioTurma ?? throw new System.ArgumentNullException(nameof(repositorioTurma));
        }
        public Task<bool> Handle(VerificaSeTurmaVirouHistoricaQuery request, CancellationToken cancellationToken)
         => repositorioTurma.VerificaSeVirouHistorica(request.TurmaId);
    }
}

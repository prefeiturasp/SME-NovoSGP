using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class VerificaSeTurmaVirouHistoricaQuery : IRequest<bool>
    {
        public long TurmaId { get; set; }

        public VerificaSeTurmaVirouHistoricaQuery(long turmaId)
        {
            TurmaId = turmaId;
        }
    }
}

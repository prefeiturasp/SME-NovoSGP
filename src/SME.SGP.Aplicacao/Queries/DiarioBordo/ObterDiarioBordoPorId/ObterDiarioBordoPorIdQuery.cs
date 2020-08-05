using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao.Queries.DiarioBordo.ObterDiarioBordoPorId
{
    public class ObterDiarioBordoPorIdQuery : IRequest<DiarioBordo>
    {

        public ObterDiarioBordoPorIdQuery(long diarioBordoId)
        {
            DiarioBordoId = diarioBordoId;
        }

        public long DiarioBordoId { get; set; }
    }
}

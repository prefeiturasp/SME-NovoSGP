using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
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

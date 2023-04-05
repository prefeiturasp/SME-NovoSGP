using System;
using System.Collections.Generic;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterAusenciaParaCompensacaoQuery : IRequest<IEnumerable<RegistroFaltasNaoCompensadaDto>>
    {
        public ObterAusenciaParaCompensacaoQuery(FiltroFaltasNaoCompensadasDto filtro)
        {
            Filtro = filtro ?? throw new ArgumentNullException(nameof(filtro));
        }

        public FiltroFaltasNaoCompensadasDto Filtro { get; set; } 
    }
}
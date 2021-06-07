using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciasAtividadeAvaliativaQuery : IRequest<IEnumerable<Aula>>
    {
        public ObterPendenciasAtividadeAvaliativaQuery(int? anoLetivo = null)
        {
            AnoLetivo = anoLetivo ?? DateTime.Today.Year;
        }
        public int AnoLetivo { get; set; }
    }
}

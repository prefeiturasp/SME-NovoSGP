using MediatR;
using SME.SGP.Dominio;
using System;

namespace SME.SGP.Aplicacao
{
    public class ObterAulaPossuiFrequenciaQuery : IRequest<bool>
    {
        public long AulaId { get; set; }
    }
}

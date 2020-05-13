using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterExistePeriodoPorUeDataBimestreQuery : IRequest<PeriodoFechamento>
    {
        public DateTime DataParaVerificar { get; internal set; }
        public long UeId { get; internal set; }
        public int Bimestre { get; internal set; }
    }
}

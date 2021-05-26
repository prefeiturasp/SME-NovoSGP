using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterValidacaoPodePersistirTurmaNasDatasQuery : IRequest<List<PodePersistirNaDataRetornoEolDto>>
    {
        public ObterValidacaoPodePersistirTurmaNasDatasQuery(string codigoRf, string turmaCodigo, DateTime[] dateTimes, long componenteCurricularCodigo)
        {
            CodigoRf = codigoRf;
            TurmaCodigo = turmaCodigo;
            DateTimes = dateTimes;
            ComponenteCurricularCodigo = componenteCurricularCodigo;
        }

        public string CodigoRf { get; }
        public string TurmaCodigo { get; }
        public DateTime[] DateTimes { get; }
        public long ComponenteCurricularCodigo { get; }
    }
}
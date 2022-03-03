using MediatR;
using SME.SGP.Infra;
using System;

namespace SME.SGP.Aplicacao
{
    public class ObterDataCriacaoRelatorioPorCodigoQuery: IRequest<DataCriacaoRelatorioDto>
    {
        public Guid CodigoRelatorio { get; set; }

        public ObterDataCriacaoRelatorioPorCodigoQuery(Guid codigoRelatorio)
        {
            CodigoRelatorio = codigoRelatorio;
        }
    }
}

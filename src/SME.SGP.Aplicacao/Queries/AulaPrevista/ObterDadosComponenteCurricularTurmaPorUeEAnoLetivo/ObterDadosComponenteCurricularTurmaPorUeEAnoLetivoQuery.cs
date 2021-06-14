using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterDadosComponenteCurricularTurmaPorUeEAnoLetivoQuery : IRequest<IEnumerable<DadosTurmaAulasAutomaticaDto>>
    {
        public ObterDadosComponenteCurricularTurmaPorUeEAnoLetivoQuery(int anoLetivo, string ueCodigo)
        {
            AnoLetivo = anoLetivo;
            UeCodigo = ueCodigo;
        }

        public int AnoLetivo { get; set; }
        public string UeCodigo { get; set; }
    }
}

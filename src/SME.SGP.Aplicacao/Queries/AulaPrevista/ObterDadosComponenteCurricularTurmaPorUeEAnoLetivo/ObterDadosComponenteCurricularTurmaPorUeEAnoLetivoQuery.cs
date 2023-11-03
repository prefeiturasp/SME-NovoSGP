using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterDadosComponenteCurricularTurmaPorUeEAnoLetivoQuery : IRequest<IEnumerable<DadosTurmaAulasAutomaticaDto>>
    {
        public ObterDadosComponenteCurricularTurmaPorUeEAnoLetivoQuery(int anoLetivo, string ueCodigo, string[] componentesCurriculares, int? semestre)
        {
            AnoLetivo = anoLetivo;
            UeCodigo = ueCodigo;
            ComponentesCurriculares = componentesCurriculares;
            Semestre = semestre;
        }

        public int AnoLetivo { get; set; }
        public string UeCodigo { get; set; }
        public string[] ComponentesCurriculares { get; set; }
        public int? Semestre { get; set; }
    }
}

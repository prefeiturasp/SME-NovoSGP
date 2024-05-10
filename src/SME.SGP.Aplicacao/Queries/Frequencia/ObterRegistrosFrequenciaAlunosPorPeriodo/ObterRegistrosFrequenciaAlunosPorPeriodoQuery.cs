using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterRegistrosFrequenciaAlunosPorPeriodoQuery : IRequest<IEnumerable<RegistroFrequenciaAlunoPorAulaDto>>
    {
        public ObterRegistrosFrequenciaAlunosPorPeriodoQuery(string turmaCodigo, string[] componentesCurricularesId, string[] alunosCodigos, DateTime dataInicio, DateTime dataFim)
        {
            TurmaCodigo = turmaCodigo;
            ComponentesCurricularesId = componentesCurricularesId;
            AlunosCodigos = alunosCodigos;
            DataInicio = dataInicio;
            DataFim = dataFim;
        }

        public string TurmaCodigo { get; }
        public string[] ComponentesCurricularesId { get; }
        public string[] AlunosCodigos { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
    }
}

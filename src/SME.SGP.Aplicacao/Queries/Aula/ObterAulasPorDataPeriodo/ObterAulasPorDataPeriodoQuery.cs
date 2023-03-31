using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterAulasPorDataPeriodoQuery : IRequest<IEnumerable<Aula>>
    {
        public ObterAulasPorDataPeriodoQuery(DateTime dataInicio, DateTime dataFim, string turmaId, string[] componentesCurricularesId, bool aulaCj, string professor = null)
        {
            DataInicio = dataInicio;
            DataFim = dataFim;
            TurmaId = turmaId;
            ComponentesCurricularesId = componentesCurricularesId;
            AulaCj = aulaCj;
            Professor = professor;
        }

        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public string TurmaId { get; set; }
        public string[] ComponentesCurricularesId { get; set; }
        public bool AulaCj { get; set; }
        public string Professor { get; set; }
    }
}

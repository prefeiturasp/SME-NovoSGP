using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterAulasPorDataPeriodoQuery : IRequest<IEnumerable<Aula>>
    {
        public ObterAulasPorDataPeriodoQuery(DateTime dataInicio, DateTime dataFim, string turmaId, string componenteCurricularId, bool aulaCj)
        {
            DataInicio = dataInicio;
            DataFim = dataFim;
            TurmaId = turmaId;
            ComponenteCurricularId = componenteCurricularId;
            AulaCj = aulaCj;
        }

        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public string TurmaId { get; set; }
        public string ComponenteCurricularId { get; set; }
        public bool AulaCj { get; set; }
    }
}

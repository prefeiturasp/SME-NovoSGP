using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterAtividadesAvaliativasPorCCTurmaPeriodoQuery : IRequest<IEnumerable<AtividadeAvaliativa>>
    {
        public ObterAtividadesAvaliativasPorCCTurmaPeriodoQuery(string[] componentesCurriculares, string turmaId, DateTime periodoInicio, DateTime periodoFim)
        {
            ComponentesCurriculares = componentesCurriculares;
            TurmaCodigo = turmaId;
            PeriodoInicio = periodoInicio;
            PeriodoFim = periodoFim;
        }

        public string[] ComponentesCurriculares { get; set; }
        public string TurmaCodigo { get; set; }
        public DateTime PeriodoInicio { get; set; }
        public DateTime PeriodoFim { get; set; }
    }
}

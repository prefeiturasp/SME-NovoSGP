using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterDadosParaConsolidacaoDashBoardFrequenciaPorTurmaQuery : IRequest<IEnumerable<DadosParaConsolidacaoDashBoardFrequenciaDto>>
    {
        public ObterDadosParaConsolidacaoDashBoardFrequenciaPorTurmaQuery(int anoLetivo, long turmaId, Modalidade modalidade, DateTime dataInicio, DateTime dataFim, DateTime? dataAula)
        {
            AnoLetivo = anoLetivo;
            TurmaId = turmaId;
            Modalidade = modalidade;
            DataInicio = dataInicio;
            DataFim = dataFim;
            DataAula = dataAula;
        }

        public int AnoLetivo { get; set; }
        public long TurmaId { get; set; }
        public Modalidade Modalidade { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public DateTime? DataAula { get; set; }
    }
}

using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasConsolidacaoFechamentoGeralPorAnoLetivoTiposEscolaQuery : IRequest<IEnumerable<TurmaConsolidacaoFechamentoGeralDto>>
    {
        public ObterTurmasConsolidacaoFechamentoGeralPorAnoLetivoTiposEscolaQuery(int anoLetivo,  int pagina = 1, int quantidadeRegistrosPorPagina = 1000, params TipoEscola[] tiposEscola)
        {
            AnoLetivo = anoLetivo == 0 ? DateTime.Today.Year : anoLetivo;
            TiposEscola = tiposEscola.EhNulo() || !tiposEscola.Any() ? new TipoEscola[] { TipoEscola.EMEF, TipoEscola.EMEFM, TipoEscola.EMEBS, TipoEscola.CEUEMEF } : tiposEscola;
            Pagina = pagina;
            QuantidadeRegistrosPorPagina = quantidadeRegistrosPorPagina;
        }

        public int AnoLetivo { get; set; }
        public TipoEscola[] TiposEscola { get; set; }
        public int Pagina { get; set; }
        public int QuantidadeRegistrosPorPagina { get; set; }
    }
}

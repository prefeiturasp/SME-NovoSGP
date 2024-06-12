using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterPareceresConclusivosAnoLetivoModalidadeQuery : IRequest<IEnumerable<ParecerConclusivoDto>>
    {
        public ObterPareceresConclusivosAnoLetivoModalidadeQuery(int anoLetivo, Modalidade modalidade)
        {
            AnoLetivo = anoLetivo;
            Modalidade = modalidade;
        }

        public int AnoLetivo { get; set; }
        public Modalidade Modalidade { get; set; }
    }
}

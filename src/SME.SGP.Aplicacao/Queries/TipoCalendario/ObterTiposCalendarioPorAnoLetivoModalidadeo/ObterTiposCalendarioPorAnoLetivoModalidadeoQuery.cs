using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterTiposCalendarioPorAnoLetivoModalidadeoQuery : IRequest<IEnumerable<TipoCalendario>>
    {
        public ObterTiposCalendarioPorAnoLetivoModalidadeoQuery(int anoLetivo, Modalidade modalidade)
        {
            AnoLetivo = anoLetivo;
            Modalidade = modalidade;
        }

        public int AnoLetivo { get; }
        public Modalidade Modalidade { get; }
    }
}

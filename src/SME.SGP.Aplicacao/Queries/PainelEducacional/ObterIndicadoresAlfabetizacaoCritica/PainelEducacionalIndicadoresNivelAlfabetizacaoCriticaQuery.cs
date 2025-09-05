using MediatR;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterIndicadoresAlfabetizacaoCritica
{
    public class PainelEducacionalIndicadoresNivelAlfabetizacaoCriticaQuery : IRequest<IEnumerable<PainelEducacionalIndicadorAlfabetizacaoCriticaDto>>
    {
        public PainelEducacionalIndicadoresNivelAlfabetizacaoCriticaQuery(string codigoDre, string codigoUe)
        {
            CodigoDre = codigoDre;
            CodigoUe = codigoUe;
        }

        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }
    }
}

﻿using MediatR;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterIndicadoresAlfabetizacaoCritica
{
    public class PainelEducacionalIndicadoresNivelAlfabetizacaoCriticaQuery : IRequest<IEnumerable<PainelEducacionalIndicadorAlfabetizacaoCriticaDto>>
    {
        public PainelEducacionalIndicadoresNivelAlfabetizacaoCriticaQuery(int anoLetivo, string codigoDre, string codigoUe)
        {
            AnoLetivo = anoLetivo;
            CodigoDre = codigoDre;
            CodigoUe = codigoUe;
        }

        public int AnoLetivo { get; set; }
        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }
    }
}

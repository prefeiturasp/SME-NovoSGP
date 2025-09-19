using MediatR;
using SME.SGP.Dominio.Entidades;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterFluenciaLeitora
{
    public class PainelEducacionalFluenciaLeitoraQuery : IRequest<IEnumerable<PainelEducacionalFluenciaLeitoraDto>>
    {
        public PainelEducacionalFluenciaLeitoraQuery(string periodo, string anoLetivo, string codigoDre, string codigoUe)
        {
            Periodo = periodo;
            AnoLetivo = anoLetivo;
            CodigoDre = codigoDre;
            CodigoUe = codigoUe;
        }

        public string Periodo { get; set; }
        public string AnoLetivo { get; set; }
        public string CodigoDre { get; set; }
        public string CodigoUe{ get; set; }
    }
}

using MediatR;
using SME.SGP.Dominio.Entidades;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterFluenciaLeitora
{
    public class PainelEducacionalFluenciaLeitoraQuery : IRequest<IEnumerable<PainelEducacionalFluenciaLeitoraDto>>
    {
        public PainelEducacionalFluenciaLeitoraQuery(int periodo, int anoLetivo, string codigoDre)
        {
            Periodo = periodo;
            AnoLetivo = anoLetivo;
            CodigoDre = codigoDre;
        }

        public int Periodo { get; set; }
        public int AnoLetivo { get; set; }
        public string CodigoDre { get; set; }
    }
}

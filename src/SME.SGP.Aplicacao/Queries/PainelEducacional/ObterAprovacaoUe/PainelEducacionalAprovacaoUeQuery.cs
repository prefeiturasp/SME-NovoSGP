using MediatR;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterAprovacaoUe
{
    public class PainelEducacionalAprovacaoUeQuery : IRequest<PainelEducacionalAprovacaoUeResultadoDto>
    {
        public PainelEducacionalAprovacaoUeQuery(int anoLetivo, string codigoUe, int numeroPagina, int numeroRegistros)
        {
            AnoLetivo = anoLetivo;
            CodigoUe = codigoUe;
            NumeroPagina = numeroPagina;
            NumeroRegistros = numeroRegistros;

        }

        public int AnoLetivo { get; set; }
        public string CodigoUe { get; set; }
        public int NumeroPagina { get; set; }
        public int NumeroRegistros { get; set; }
    }
}

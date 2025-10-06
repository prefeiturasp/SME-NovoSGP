using MediatR;
using SME.SGP.Infra.Dtos.PainelEducacional.SondagemEscrita;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterSondagemEscrita
{
    public class ObterSondagemEscritaQuery : IRequest<IEnumerable<SondagemEscritaDto>>
    {
        public ObterSondagemEscritaQuery(string codigoDre, string codigoUe, int anoLetivo, int bimestre, int serieAno)
        {
            CodigoDre = codigoDre;
            CodigoUe = codigoUe;
            AnoLetivo = anoLetivo;
            Bimestre = bimestre;
            SerieAno = serieAno;
        }
        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }
        public int AnoLetivo { get; set; }
        public int Bimestre { get; set; }
        public int SerieAno { get; set; }
    }
}

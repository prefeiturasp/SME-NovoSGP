using MediatR;
using SME.SGP.Infra.Dtos.PainelEducacional.Notas.VisaoUe;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterNotas.VisaoUe
{
    public class ObterModalidadesNotasVisaoUeQuery : IRequest<IEnumerable<IdentificacaoInfo>>
    {
        public ObterModalidadesNotasVisaoUeQuery(int anoLetivo, string codigoUe, int bimestre)
        {
            AnoLetivo = anoLetivo;
            CodigoUe = codigoUe;
            Bimestre = bimestre;
        }

        public int AnoLetivo { get; set; }
        public string CodigoUe { get; set; }
        public int Bimestre { get; set; }
    }
}

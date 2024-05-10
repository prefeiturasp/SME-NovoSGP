using System.Collections.Generic;
using MediatR;
using SME.SGP.Infra.Dtos;

namespace SME.SGP.Aplicacao
{
    public class ObterTotalDisciplinasFechamentoPorTurmaQuery : IRequest<IEnumerable<TurmaFechamentoDisciplinaDto>>
    {
        public int AnoLetivo { get; set; }
        public int Bimestre { get; set; }
        public ObterTotalDisciplinasFechamentoPorTurmaQuery(int anoLetivo, int bimestre)
        {
            AnoLetivo = anoLetivo;
            Bimestre = bimestre;
        }
    }
}
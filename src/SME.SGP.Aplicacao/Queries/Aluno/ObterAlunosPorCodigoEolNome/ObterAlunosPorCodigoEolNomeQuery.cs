using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunosPorCodigoEolNomeQuery : IRequest<IEnumerable<AlunoSimplesDto>>
    {
        public ObterAlunosPorCodigoEolNomeQuery(FiltroBuscaEstudanteDto filtroEstudante)
        {
            CodigoUe = filtroEstudante.CodigoUe;
            AnoLetivo = filtroEstudante.AnoLetivo;
            CodigoEOL = filtroEstudante.Codigo;
            Nome = filtroEstudante.Nome;
            CodigoTurma = filtroEstudante.CodigoTurma; ;
        }

        public string CodigoUe { get; set; }
        public string AnoLetivo { get; set; }
        public long? CodigoEOL { get; set; }
        public long CodigoTurma { get; set; }
        public string Nome { get; set; }
    }
}

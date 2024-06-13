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
            CodigoTurmas = filtroEstudante.CodigoTurmas;
            SomenteAtivos = true;
        }

        public string CodigoUe { get; set; }
        public string AnoLetivo { get; set; }
        public long? CodigoEOL { get; set; }
        public long[] CodigoTurmas { get; set; }
        public string Nome { get; set; }
        public bool SomenteAtivos { get; set; }
    }
}

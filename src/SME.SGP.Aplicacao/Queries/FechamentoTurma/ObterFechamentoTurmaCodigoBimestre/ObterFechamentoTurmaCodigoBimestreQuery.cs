using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterFechamentoTurmaCodigoBimestreQuery : IRequest<FechamentoTurma>
    {
        public ObterFechamentoTurmaCodigoBimestreQuery(string turmaCodigo, int bimestre)
        {
            TurmaCodigo = turmaCodigo;
            Bimestre = bimestre;
        }

        public string TurmaCodigo { get; set; }
        public int Bimestre { get; set; }
    }
}

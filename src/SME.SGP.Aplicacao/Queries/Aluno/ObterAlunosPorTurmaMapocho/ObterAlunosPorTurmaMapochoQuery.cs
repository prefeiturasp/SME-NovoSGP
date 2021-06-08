using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunosPorTurmaMapochoQuery : IRequest<string[]>
    {
        public string TurmaCodigo { get; set; }

        public ObterAlunosPorTurmaMapochoQuery(string turmaCodigo)
        {
            TurmaCodigo = turmaCodigo;
        }
    }
}

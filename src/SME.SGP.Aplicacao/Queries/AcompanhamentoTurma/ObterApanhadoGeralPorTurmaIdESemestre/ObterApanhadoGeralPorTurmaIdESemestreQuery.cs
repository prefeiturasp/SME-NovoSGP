using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterApanhadoGeralPorTurmaIdESemestreQuery : IRequest<AcompanhamentoTurma>
    {
        public ObterApanhadoGeralPorTurmaIdESemestreQuery(long turmaId, int semestre)
        {
            TurmaId = turmaId;
            Semestre = semestre;
        }

        public long TurmaId { get; }
        public int Semestre { get; }
    }
}

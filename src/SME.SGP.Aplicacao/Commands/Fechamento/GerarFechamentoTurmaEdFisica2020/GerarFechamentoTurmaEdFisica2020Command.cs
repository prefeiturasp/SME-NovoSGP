using MediatR;

namespace SME.SGP.Aplicacao
{
    public class GerarFechamentoTurmaEdFisica2020Command : IRequest<bool>
    {
        public long TurmaId { get; set; }
        public long[] CodigoAlunos { get; set; }

        public GerarFechamentoTurmaEdFisica2020Command(long turmaId, long[] codigoAlunos)
        {
            TurmaId = turmaId;
            CodigoAlunos = codigoAlunos;
        }
    }
}

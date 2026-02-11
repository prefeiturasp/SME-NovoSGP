using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterAvaliacoesBimestraisAlunoQuery : IRequest<AvaliacoesBimestraisAlunoDto>
    {
        public ObterAvaliacoesBimestraisAlunoQuery(string codigoAluno, int anoLetivo)
        {
            CodigoAluno = codigoAluno;
            AnoLetivo = anoLetivo;
        }

        public string CodigoAluno { get; }
        public int AnoLetivo { get; }
    }
}
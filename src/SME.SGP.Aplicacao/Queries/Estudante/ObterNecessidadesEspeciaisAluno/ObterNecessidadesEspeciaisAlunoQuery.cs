using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterNecessidadesEspeciaisAlunoQuery : IRequest<InformacoesEscolaresAlunoDto>
    {
        public int CodigoAluno { get; set; }

        public ObterNecessidadesEspeciaisAlunoQuery(int codigoAluno)
        {
            CodigoAluno = codigoAluno;
        }
    }
}
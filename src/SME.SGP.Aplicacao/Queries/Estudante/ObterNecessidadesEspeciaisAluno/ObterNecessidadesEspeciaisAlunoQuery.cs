using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterNecessidadesEspeciaisAlunoQuery : IRequest<InformacoesEscolaresAlunoDto>
    {

        public ObterNecessidadesEspeciaisAlunoQuery(int codigoAluno, string turmaId)
        {
            CodigoAluno = codigoAluno;
            TurmaCodigo = turmaId;
        }
        public int CodigoAluno { get; set; }
        public string TurmaCodigo { get; set; }
    }
}
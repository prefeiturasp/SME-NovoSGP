using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterNecessidadesEspeciaisAlunoQuery : IRequest<InformacoesEscolaresAlunoDto>
    {

        public ObterNecessidadesEspeciaisAlunoQuery(string codigoAluno, string turmaId)
        {
            CodigoAluno = codigoAluno;
            TurmaCodigo = turmaId;
        }
        public string CodigoAluno { get; set; }
        public string TurmaCodigo { get; set; }
    }
}
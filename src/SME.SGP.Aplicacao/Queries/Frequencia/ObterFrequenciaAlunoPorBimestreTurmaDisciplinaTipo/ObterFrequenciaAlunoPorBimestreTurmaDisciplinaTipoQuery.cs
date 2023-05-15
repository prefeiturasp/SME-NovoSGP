using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciaAlunoPorBimestreTurmaDisciplinaTipoQuery : IRequest<FrequenciaAluno>
    {
        public string CodigoAluno { get; set; }
        public int Bimestre { get; set; }
        public TipoFrequenciaAluno TipoFrequencia { get; set; }
        public string CodigoTurma { get; set; }
        public string[] DisciplinasId { get; set; }
        public string Professor { get; set; }

        public ObterFrequenciaAlunoPorBimestreTurmaDisciplinaTipoQuery(string codigoAluno, int bimestre, TipoFrequenciaAluno tipoFrequencia, string codigoTurma, string[] disciplinasId = null, string professor = null)
        {
            CodigoAluno = codigoAluno;
            Bimestre = bimestre;
            TipoFrequencia = tipoFrequencia;
            CodigoTurma = codigoTurma;
            DisciplinasId = disciplinasId;
            Professor = professor;
        }
    }

    public class ObterFrequenciaAlunoPorBimestreTurmaDisciplinaTipoQueryValidator : AbstractValidator<ObterFrequenciaAlunoPorBimestreTurmaDisciplinaTipoQuery>
    {
        public ObterFrequenciaAlunoPorBimestreTurmaDisciplinaTipoQueryValidator()
        {
            RuleFor(a => a.CodigoAluno)
                .NotEmpty()
                .WithMessage("É necessário informar o código do aluno para consulta das frequências.");
            RuleFor(a => a.CodigoAluno)
               .NotEmpty()
               .WithMessage("É necessário informar o bimestre para consulta das frequências.");
            RuleFor(a => a.CodigoAluno)
               .NotEmpty()
               .WithMessage("É necessário informar o código da turma para consulta das frequências.");
        }
    }
}

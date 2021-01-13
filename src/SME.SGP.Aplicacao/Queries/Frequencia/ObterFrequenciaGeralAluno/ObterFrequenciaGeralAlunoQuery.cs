using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciaGeralAlunoQuery : IRequest<double>
    {
        public ObterFrequenciaGeralAlunoQuery(int codigoAluno, string codigoTurma)
        {
            CodigoAluno = codigoAluno;
            CodigoTurma = codigoTurma;
        }

        public int CodigoAluno { get; set; }
        public string CodigoTurma { get; set; }        
    }

    public class ObterFrequenciaGeralAlunoQueryValidator : AbstractValidator<ObterFrequenciaGeralAlunoQuery>
    {
        public ObterFrequenciaGeralAlunoQueryValidator()
        {
            RuleFor(a => a.CodigoAluno)
                .NotEmpty()
                .WithMessage("Necessário informar o Código do Aluno");

            RuleFor(a => a.CodigoTurma)
                .NotEmpty()
                .WithMessage("Necessário informar o Código da Turma");
        }
    }
}

using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ExisteAtendimentoNAAPAAtivoParaAlunoQuery : IRequest<bool>
    {
        public ExisteAtendimentoNAAPAAtivoParaAlunoQuery(string codigoAluno)
        {
            CodigoAluno = codigoAluno;
        }

        public string CodigoAluno { get; set; } 
    }

    public class ExisteAtendimentoNAAPAAtivoParaAlunoQueryValidator : AbstractValidator<ExisteAtendimentoNAAPAAtivoParaAlunoQuery>
    {
        public ExisteAtendimentoNAAPAAtivoParaAlunoQueryValidator()
        {
            RuleFor(c => c.CodigoAluno)
                .NotEmpty()
                .WithMessage("O código do aluno deve ser informado para a verificação de existência do atendimento naapa para o aluno.");
        }
    }
}

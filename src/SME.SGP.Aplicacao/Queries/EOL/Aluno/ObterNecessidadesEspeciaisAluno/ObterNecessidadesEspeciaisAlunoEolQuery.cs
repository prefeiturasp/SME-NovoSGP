using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterNecessidadesEspeciaisAlunoEolQuery : IRequest<InformacoesEscolaresAlunoDto>
    {
        public ObterNecessidadesEspeciaisAlunoEolQuery(string codigoAluno)
        {
            this.CodigoAluno = codigoAluno;
        }

        public string CodigoAluno { get; set; }
    }

    public class ObterNecessidadesEspeciaisAlunoEolQueryValidator : AbstractValidator<ObterNecessidadesEspeciaisAlunoEolQuery>
    {
        public ObterNecessidadesEspeciaisAlunoEolQueryValidator()
        {
            RuleFor(x => x.CodigoAluno)
                .NotEmpty()
                .WithMessage("O código do aluno deve ser informado para obter informações da necessidade especial.");
        }
    }
}

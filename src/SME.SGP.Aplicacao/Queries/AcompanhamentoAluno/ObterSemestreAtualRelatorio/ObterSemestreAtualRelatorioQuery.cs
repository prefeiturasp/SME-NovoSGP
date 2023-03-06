using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ObterSemestreAtualRelatorioQuery : IRequest<int>
    {
        public string AlunoCodigo { get; set; }

        public ObterSemestreAtualRelatorioQuery(string alunoCodigo)
        {
            AlunoCodigo = alunoCodigo;
        }
    }

    public class ObterSemestreAtualRelatorioQueryValidator : AbstractValidator<ObterSemestreAtualRelatorioQuery>
    {
        public ObterSemestreAtualRelatorioQueryValidator()
        {
            RuleFor(a => a.AlunoCodigo)
                .NotEmpty()
                .WithMessage("O Codigo do Aluno deve ser informado");
        }
    }
}

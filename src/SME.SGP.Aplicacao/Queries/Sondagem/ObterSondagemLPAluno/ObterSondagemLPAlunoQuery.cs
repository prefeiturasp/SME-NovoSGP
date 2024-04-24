using FluentValidation;
using MediatR;
using SME.SGP.Infra.Dtos.Sondagem;

namespace SME.SGP.Aplicacao.Queries
{
    public class ObterSondagemLPAlunoQuery : IRequest<SondagemLPAlunoDto>
    {
        public ObterSondagemLPAlunoQuery(string turmaCodigo, string alunoCodigo)
        {
            TurmaCodigo = turmaCodigo;
            AlunoCodigo = alunoCodigo;
        }

        public string TurmaCodigo { get; }
        public string AlunoCodigo { get; }
    }

    public class ObterSondagemLPAlunoQueryValidator : AbstractValidator<ObterSondagemLPAlunoQuery>
    {
        public ObterSondagemLPAlunoQueryValidator()
        {
            RuleFor(x => x.TurmaCodigo)
                .NotEmpty()
                .WithMessage("A turma do aluno deve ser informada para pesquisa no Sondagem");

            RuleFor(x => x.AlunoCodigo)
                .NotEmpty()
                .WithMessage("O código do aluno deve ser informado para pesquisa no Sondagem");
        }
    }
}

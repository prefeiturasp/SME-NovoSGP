using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao.Queries.HistoricoEscolarObservacao
{
    public class ObterHistoricoEscolarObservacaoPorAlunoQuery : IRequest<Dominio.HistoricoEscolarObservacao>
    {
        public ObterHistoricoEscolarObservacaoPorAlunoQuery(string alunoCodigo)
        {
            AlunoCodigo = alunoCodigo;
        }

        public string AlunoCodigo { get; }
    }

    public class ObterHistoricoEscolarObservacaoPorAlunoQueryValidator : AbstractValidator<ObterHistoricoEscolarObservacaoPorAlunoQuery>
    {
        public ObterHistoricoEscolarObservacaoPorAlunoQueryValidator()
        {
            RuleFor(f => f.AlunoCodigo)
                .NotEmpty()
                .WithMessage("O Código do Aluno deve ser informado.");
        }
    }
}

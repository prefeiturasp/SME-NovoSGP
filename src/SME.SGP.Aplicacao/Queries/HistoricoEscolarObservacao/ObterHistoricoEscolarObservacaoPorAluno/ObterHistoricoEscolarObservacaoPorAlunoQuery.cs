using FluentValidation;
using MediatR;
using SME.SGP.Infra.Dtos;

namespace SME.SGP.Aplicacao.Queries.HistoricoEscolarObservacao
{
    public class ObterHistoricoEscolarObservacaoPorAlunoQuery : IRequest<HistoricoEscolarObservacaoDto>
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

using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterPlanoAEETurmaAlunoPorIdQuery : IRequest<IEnumerable<PlanoAEETurmaAluno>>
    {
        public ObterPlanoAEETurmaAlunoPorIdQuery(long planoAEEId)
        {
            PlanoAEEId = planoAEEId;
        }

        public long PlanoAEEId { get; }
    }

    public class ObterPlanoAEETurmaAlunoQueryPorIdValidator : AbstractValidator<ObterPlanoAEETurmaAlunoPorIdQuery>
    {
        public ObterPlanoAEETurmaAlunoQueryPorIdValidator()
        {
            RuleFor(t => t.PlanoAEEId)
                .NotEmpty()
                .WithMessage("O código do plano deve ser informado para obter as turmas do aluno");
        }
    }
}

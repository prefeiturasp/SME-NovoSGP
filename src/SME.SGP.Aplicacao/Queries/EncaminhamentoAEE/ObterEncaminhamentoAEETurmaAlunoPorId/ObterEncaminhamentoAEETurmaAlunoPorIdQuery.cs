using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterEncaminhamentoAEETurmaAlunoPorIdQuery : IRequest<IEnumerable<EncaminhamentoAEETurmaAluno>>
    {
        public ObterEncaminhamentoAEETurmaAlunoPorIdQuery(long encaminhamentoAEEId)
        {
            EncaminhamentoAEEId = encaminhamentoAEEId;
        }

        public long EncaminhamentoAEEId { get; }
    }

    public class ObterEncaminhamentoAEETurmaAlunoPorIdQueryValidator: AbstractValidator<ObterEncaminhamentoAEETurmaAlunoPorIdQuery>
    {
        public ObterEncaminhamentoAEETurmaAlunoPorIdQueryValidator()
        {
            RuleFor(t => t.EncaminhamentoAEEId)
                .NotEmpty()
                .WithMessage("O id do encaminhamento aee deve ser informado para obter as turmas do encaminhamento aee");
        }
    }
}

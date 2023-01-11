using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterOpcoesRespostaPorQuestaoIdQuery : IRequest<IEnumerable<OpcaoRespostaSimplesDto>>
    {
        public ObterOpcoesRespostaPorQuestaoIdQuery(long id)
        {
            this.Id = id;
        }

        public long Id { get; }
    }

    public class ObterOpcaoRespostaPorQuestaoIdQueryValidator : AbstractValidator<ObterOpcoesRespostaPorQuestaoIdQuery>
    {
        public ObterOpcaoRespostaPorQuestaoIdQueryValidator()
        {
            RuleFor(a => a.Id)
                .NotEmpty()
                .WithMessage("O id da opção de resposta deve ser informado para retorno de suas informações.");
        }
    }
}

using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterQuestaoRespostaEncaminhamentoNAAPAPorIdQuery : IRequest<IEnumerable<RespostaQuestaoAtendimentoNAAPADto>>
    {
        public ObterQuestaoRespostaEncaminhamentoNAAPAPorIdQuery(long id)
        {
            Id = id;
        }

        public long Id { get; }
    }

    public class ObterQuestaoRespostaEncaminhamentoNAAPAPorIdQueryValidator : AbstractValidator<ObterQuestaoRespostaEncaminhamentoNAAPAPorIdQuery>
    {
        public ObterQuestaoRespostaEncaminhamentoNAAPAPorIdQueryValidator()
        {
            RuleFor(c => c.Id)
            .NotEmpty()
            .WithMessage("O Id da resposta do encaminhamento naapa deve ser informado para a pesquisa");

        }
    }
}

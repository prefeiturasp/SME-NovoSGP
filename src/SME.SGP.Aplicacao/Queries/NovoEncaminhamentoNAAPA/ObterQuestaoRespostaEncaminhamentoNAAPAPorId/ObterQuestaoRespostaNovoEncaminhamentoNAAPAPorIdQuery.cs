using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.NovoEncaminhamentoNAAPA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.NovoEncaminhamentoNAAPA.ObterQuestaoRespostaEncaminhamentoNAAPAPorId
{
    public class ObterQuestaoRespostaNovoEncaminhamentoNAAPAPorIdQuery : IRequest<IEnumerable<RespostaQuestaoNovoEncaminhamentoNAAPADto>>
    {
        public ObterQuestaoRespostaNovoEncaminhamentoNAAPAPorIdQuery(long id)
        {
            Id = id;
        }

        public long Id { get; }
    }

    public class ObterQuestaoRespostaNovoEncaminhamentoNAAPAPorIdQueryValidator : AbstractValidator<ObterQuestaoRespostaNovoEncaminhamentoNAAPAPorIdQuery>
    {
        public ObterQuestaoRespostaNovoEncaminhamentoNAAPAPorIdQueryValidator()
        {
            RuleFor(c => c.Id)
            .NotEmpty()
            .WithMessage("O Id da resposta do encaminhamento naapa deve ser informado para a pesquisa");

        }
    }
}
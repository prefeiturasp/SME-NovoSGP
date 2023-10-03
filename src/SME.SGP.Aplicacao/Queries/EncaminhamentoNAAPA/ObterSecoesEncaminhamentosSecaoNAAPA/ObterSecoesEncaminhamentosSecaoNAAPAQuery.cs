using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries
{
    public class ObterSecoesEncaminhamentosSecaoNAAPAQuery : IRequest<IEnumerable<SecaoQuestionarioDto>>
    {
        public ObterSecoesEncaminhamentosSecaoNAAPAQuery(int[] modalidades, long? encaminhamentoNAAPAId)
        {
            EncaminhamentoNAAPAId = encaminhamentoNAAPAId;
            Modalidades = modalidades;
        }

        public long? EncaminhamentoNAAPAId { get; }
        public int[] Modalidades { get; }
    }

    public class ObterSecoesEncaminhamentosSecaoNAAPAQueryValidator : AbstractValidator<ObterSecoesEncaminhamentosSecaoNAAPAQuery>
    {
        public ObterSecoesEncaminhamentosSecaoNAAPAQueryValidator()
        {
            RuleFor(c => c.Modalidades)
                .NotNull()
                .WithMessage("As modalidades devem ser informadas para obter as seções/encaminhamentos seção do encaminhamento NAAPA.");
        }
    }

}

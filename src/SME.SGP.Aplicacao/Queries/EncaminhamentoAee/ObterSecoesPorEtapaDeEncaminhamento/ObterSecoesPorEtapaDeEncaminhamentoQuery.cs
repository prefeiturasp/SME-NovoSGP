using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries
{
    public class ObterSecoesPorEtapaDeEncaminhamentoQuery : IRequest<IEnumerable<SecaoQuestionarioDto>>
    {
        public long EncaminhamentoAeeId { get; set; }

        public ObterSecoesPorEtapaDeEncaminhamentoQuery(long encaminhamentoAeeId)
        {
            EncaminhamentoAeeId = encaminhamentoAeeId;
        }
    }

    public class ObterSecoesPorEtapaDeEncaminhamentoQueryValidator : AbstractValidator<ObterSecoesPorEtapaDeEncaminhamentoQuery>
    {
        public ObterSecoesPorEtapaDeEncaminhamentoQueryValidator()
        {
            RuleFor(c => c.EncaminhamentoAeeId)
            .NotEmpty()
            .WithMessage("O Id deve ser informado para pesquisa do Encaminhamento AEE");

        }
    }

}

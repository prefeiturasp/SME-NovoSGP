using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries
{
    public class ObterSecoesPorEtapaDeEncaminhamentoNAAPAQuery : IRequest<IEnumerable<SecaoQuestionarioDto>>
    {
        public ObterSecoesPorEtapaDeEncaminhamentoNAAPAQuery(List<int> etapas, long encaminhamentoNAAPAId)
        {
            Etapas = etapas;
            EncaminhamentoNAAPAId = encaminhamentoNAAPAId;
        }

        public List<int> Etapas { get; set; }
        public long EncaminhamentoNAAPAId { get; }

    }

    public class ObterSecoesPorEtapaDeEncaminhamentoNAAPAQueryValidator : AbstractValidator<ObterSecoesPorEtapaDeEncaminhamentoNAAPAQuery>
    {
        public ObterSecoesPorEtapaDeEncaminhamentoNAAPAQueryValidator()
        {
            RuleFor(c => c.Etapas)
            .NotEmpty()
            .WithMessage("As Etapas devem ser informadas.");
        }
    }

}

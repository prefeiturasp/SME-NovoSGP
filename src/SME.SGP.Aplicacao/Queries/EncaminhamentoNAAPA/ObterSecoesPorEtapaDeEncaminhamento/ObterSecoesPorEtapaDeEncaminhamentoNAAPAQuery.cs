using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries
{
    public class ObterSecoesPorEtapaDeEncaminhamentoNAAPAQuery : IRequest<IEnumerable<SecaoQuestionarioDto>>
    {
        public ObterSecoesPorEtapaDeEncaminhamentoNAAPAQuery(List<int> etapas, long? encaminhamentoNAAPAId, int modalidade)
        {
            Etapas = etapas;
            EncaminhamentoNAAPAId = encaminhamentoNAAPAId;
            Modalidade = modalidade;
        }

        public List<int> Etapas { get; set; }
        public long? EncaminhamentoNAAPAId { get; }
        public int Modalidade { get; }
    }

    public class ObterSecoesPorEtapaDeEncaminhamentoNAAPAQueryValidator : AbstractValidator<ObterSecoesPorEtapaDeEncaminhamentoNAAPAQuery>
    {
        public ObterSecoesPorEtapaDeEncaminhamentoNAAPAQueryValidator()
        {
            RuleFor(c => c.Etapas)
                .NotEmpty()
                .WithMessage("As Etapas devem ser informadas para obter as seções por etapa do encaminhamento NAAPA.");
            
            RuleFor(c => c.Modalidade)
                .NotEmpty()
                .WithMessage("A modalidade deve ser informada para obter as seções por etapa do encaminhamento NAAPA.");
        }
    }

}

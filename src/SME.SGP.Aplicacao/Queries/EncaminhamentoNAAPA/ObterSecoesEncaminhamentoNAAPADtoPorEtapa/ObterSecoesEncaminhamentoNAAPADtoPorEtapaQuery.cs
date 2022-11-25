using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries
{
    public class ObterSecoesEncaminhamentoNAAPADtoPorEtapaQuery : IRequest<IEnumerable<SecaoQuestionarioDto>>
    {
        public ObterSecoesEncaminhamentoNAAPADtoPorEtapaQuery(int etapa)
        {
            Etapa = etapa;
        }

        public int Etapa { get; set; }
    }

    public class ObterSecoesEncaminhamentoNAAPADtoPorEtapaQueryValidator : AbstractValidator<ObterSecoesEncaminhamentoNAAPADtoPorEtapaQuery>
    {
        public ObterSecoesEncaminhamentoNAAPADtoPorEtapaQueryValidator()
        {
            RuleFor(c => c.Etapa)
            .NotEmpty()
            .WithMessage("Etapa deve ser informada para obtenção das seções/questionários ativos para encaminhamento NAAPA.");
        }
    }

}

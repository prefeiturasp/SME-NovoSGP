using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries
{
    public class ObterSecoesEncaminhamentoDtoPorEtapaQuery : IRequest<IEnumerable<SecaoQuestionarioDto>>
    {
        public ObterSecoesEncaminhamentoDtoPorEtapaQuery(int etapa)
        {
            Etapa = etapa;
        }

        public int Etapa { get; set; }
        

    }

    public class ObterSecoesEncaminhamentoDtoPorEtapaQueryValidator : AbstractValidator<ObterSecoesEncaminhamentoDtoPorEtapaQuery>
    {
        public ObterSecoesEncaminhamentoDtoPorEtapaQueryValidator()
        {
            RuleFor(c => c.Etapa)
            .NotEmpty()
            .WithMessage("Etapa deve ser informada para obtenção das seções/questionários ativos.");
        }
    }

}

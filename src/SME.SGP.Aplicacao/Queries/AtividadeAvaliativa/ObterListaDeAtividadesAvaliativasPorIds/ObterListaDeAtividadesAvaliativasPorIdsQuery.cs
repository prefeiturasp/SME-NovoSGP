using System.Collections.Generic;
using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterListaDeAtividadesAvaliativasPorIdsQuery : IRequest<IEnumerable<AtividadeAvaliativa>>
    {
        public ObterListaDeAtividadesAvaliativasPorIdsQuery(IEnumerable<long> atividadesAvaliativasIds)
        {
            AtividadesAvaliativasIds = atividadesAvaliativasIds;
        }
        public IEnumerable<long> AtividadesAvaliativasIds { get; set; }
    }

    public class ObterListaDeAtividadesAvaliativasPorIdsQueryValidator : AbstractValidator<
            ObterListaDeAtividadesAvaliativasPorIdsQuery>
    {
        public ObterListaDeAtividadesAvaliativasPorIdsQueryValidator()
        {
            RuleFor(a => a.AtividadesAvaliativasIds)
                .NotEmpty().WithMessage("É necessário informar uma Atividade Avaliativa Id para Obter Lista De Atividades Avaliativas ");
        }
    }
}
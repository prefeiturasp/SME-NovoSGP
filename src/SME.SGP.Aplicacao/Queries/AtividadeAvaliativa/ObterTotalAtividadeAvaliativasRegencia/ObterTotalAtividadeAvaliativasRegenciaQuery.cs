using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterTotalAtividadeAvaliativasRegenciaQuery : IRequest<IEnumerable<ComponentesRegenciaComAtividadeAvaliativaDto>>
    {
        public ObterTotalAtividadeAvaliativasRegenciaQuery(long[] atividadesAvaliativasId)
        {
            AtividadesAvaliativasId = atividadesAvaliativasId;
        }

        public long[] AtividadesAvaliativasId { get; set; }
    }

    public class ObterTotalAtividadeAvaliativasRegenciaQueryValidator : AbstractValidator<ObterTotalAtividadeAvaliativasRegenciaQuery>
    {
        public ObterTotalAtividadeAvaliativasRegenciaQueryValidator()
        {
            RuleFor(a => a.AtividadesAvaliativasId)
               .NotNull()
               .WithMessage("Os ids de atividade avaliativas são obrigatórios para obter o total de componentes com atividade avaliativa.");
        }
    }
}

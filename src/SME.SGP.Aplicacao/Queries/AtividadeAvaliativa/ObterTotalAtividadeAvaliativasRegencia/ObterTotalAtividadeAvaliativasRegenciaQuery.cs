using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterTotalAtividadeAvaliativasRegenciaQuery : IRequest<TotalizadorAtividadesAvaliativasRegenciaDto>
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

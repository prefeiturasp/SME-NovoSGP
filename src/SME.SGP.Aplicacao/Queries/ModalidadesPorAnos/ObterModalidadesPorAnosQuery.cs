using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterModalidadesPorAnosQuery : IRequest<IEnumerable<ModalidadesPorAnoDto>>
    {
        public List<string> Anos { get; set; }

        public ObterModalidadesPorAnosQuery(List<string> anos)
        {
            Anos = anos;
        }
    }

    public class ObterModalidadesPorAnosQueryValidator : AbstractValidator<ObterModalidadesPorAnosQuery>
    {
        public ObterModalidadesPorAnosQueryValidator()
        {
            RuleFor(c => c)
                .NotEmpty()
                .WithMessage("É obrigatório informar ao menos 1 ano");
        }
    }
}

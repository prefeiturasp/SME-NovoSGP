using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ConsolidarRegistrosPedagogicosCommand : IRequest
    {
        public ConsolidarRegistrosPedagogicosCommand(ConsolidacaoRegistrosPedagogicos consolidacao)
        {
            ConsolidacaoRegistrosPedagogicos = consolidacao;
        }
        public ConsolidacaoRegistrosPedagogicos ConsolidacaoRegistrosPedagogicos { get; set; }

    }

    public class ConsolidarRegistrosPedagogicosCommandValidator : AbstractValidator<ConsolidarRegistrosPedagogicosCommand>
    {
        public ConsolidarRegistrosPedagogicosCommandValidator()
        {
            RuleFor(a => a.ConsolidacaoRegistrosPedagogicos)
                .NotEmpty()
                .WithMessage("Os dados da consolidação devem ser informados para o registro.");
        }
    }
}

using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;

namespace SME.SGP.Aplicacao
{
    public class AlterarCartaIntencoesCommand : IRequest<AuditoriaDto>
    {
        public CartaIntencoes Existente { get; set; }

        public long TurmaId { get; set; }

        public long ComponenteCurricularId { get; set; }

        public CartaIntencoesDto Carta { get; set; }

        public AlterarCartaIntencoesCommand(CartaIntencoesDto carta, CartaIntencoes existente, long turmaId, long componenteCurricularId)
        {
            Carta = carta;
            Existente = existente;
            TurmaId = turmaId;
            ComponenteCurricularId = componenteCurricularId;
        }
    }
    public class AlterarCartaIntencoesCommandValidator : AbstractValidator<AlterarCartaIntencoesCommand>
    {
        public AlterarCartaIntencoesCommandValidator()
        {
            RuleFor(a => a.Carta.Id)
                   .GreaterThan(0)
                   .WithMessage("O id da carta de intenções deve ser informado!");

            RuleFor(a => a.Carta.Planejamento)
                   .NotEmpty().WithMessage("O planejamento é obrigatório para o registro da carta de intenções!");

        }
    }
}

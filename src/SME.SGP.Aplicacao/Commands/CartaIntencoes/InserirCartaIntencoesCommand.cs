using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;

namespace SME.SGP.Aplicacao
{
    public class InserirCartaIntencoesCommand : IRequest<AuditoriaDto>
    {
        public CartaIntencoesDto Carta { get; set; }

        public InserirCartaIntencoesCommand(CartaIntencoesDto carta)
        {
            Carta = carta;
        }
    }
    public class InserirCartaIntencoesCommandValidator : AbstractValidator<InserirCartaIntencoesCommand>
    {
        public InserirCartaIntencoesCommandValidator()
        {
            RuleFor(x => x.Carta.TurmaId)
                   .GreaterThan(0)
                   .WithMessage("A turma deve ser informada!");

            RuleFor(a => a.Carta.PeriodoEscolarId)
                   .GreaterThan(0)
                   .WithMessage("O período escolar deve ser informado!");

            RuleFor(a => a.Carta.Bimestre)
                   .GreaterThan(0)
                   .WithMessage("O bimestre deve ser informado!");

            RuleFor(a => a.Carta.Planejamento)
                   .NotEmpty().WithMessage("O planejamento é obrigatório para o registro da carta de intenções!");

        }
    }
}

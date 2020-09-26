using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;

namespace SME.SGP.Aplicacao
{
    public class InserirCartaIntencoesCommand : IRequest<AuditoriaDto>
    {
        public long TurmaId { get; set; }

        public long ComponenteCurricularId { get; set; }

        public CartaIntencoesDto Carta { get; set; }

        public InserirCartaIntencoesCommand(CartaIntencoesDto carta, long turmaId, long componenteCurricularId)
        {
            Carta = carta;
            TurmaId = turmaId;
            ComponenteCurricularId = componenteCurricularId;
        }
    }
    public class InserirCartaIntencoesCommandValidator : AbstractValidator<InserirCartaIntencoesCommand>
    {
        public InserirCartaIntencoesCommandValidator()
        {
            RuleFor(x => x.TurmaId)
                   .GreaterThan(0)
                   .WithMessage("A turma deve ser informada!");

            RuleFor(x => x.ComponenteCurricularId)
                   .GreaterThan(0)
                   .WithMessage("O componente curricular deve ser informado!");

            RuleFor(a => a.Carta.PeriodoEscolarId)
                   .GreaterThan(0)
                   .WithMessage("O período escolar deve ser informado!");

            RuleFor(a => a.Carta.Planejamento)
                   .NotEmpty().WithMessage("O planejamento é obrigatório para o registro da carta de intenções!");

        }
    }
}

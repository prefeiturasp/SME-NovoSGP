using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class SalvarItineranciaObjetivoCommand : IRequest<AuditoriaDto>
    {
        public SalvarItineranciaObjetivoCommand(long itineranciaObjetivoBaseId, long itineranciaId, string descricao, bool temDescricao)
        {
            ItineranciaObjetivoBaseId = itineranciaObjetivoBaseId;
            ItineranciaId = itineranciaId;
            Descricao = descricao;
            TemDescricao = temDescricao;
        }

        public long ItineranciaObjetivoBaseId { get; set; }
        public long ItineranciaId { get; set; }
        public string Descricao { get; set; }
        public bool TemDescricao { get; set; }
    }
    public class SalvarItineranciaObjetivoCommandValidator : AbstractValidator<SalvarItineranciaObjetivoCommand>
    {
        public SalvarItineranciaObjetivoCommandValidator()
        {
            RuleFor(x => x.ItineranciaObjetivoBaseId)
                   .GreaterThan(0)
                   .WithMessage("O id do objetivo base deve ser informado!");
            RuleFor(x => x.ItineranciaId)
                   .GreaterThan(0)
                   .WithMessage("O id da itinerância deve ser informado!");
            RuleFor(x => x.Descricao)
                   .NotEmpty()
                   .WithMessage("A descrição do objetivo deve ser informada!")
                   .When(x => x.TemDescricao);
        }
    }
}

using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class SalvarPendenciaCalendarioUeCommand : IRequest<long>
    {
        public SalvarPendenciaCalendarioUeCommand(long tipoCalendarioId, Ue ue, string descricao, string instrucao, TipoPendencia tipoPendencia)
        {
            TipoCalendarioId = tipoCalendarioId;
            Ue = ue;
            Descricao = descricao;
            Instrucao = instrucao;
            TipoPendencia = tipoPendencia;
        }

        public long TipoCalendarioId { get; set; }
        public Ue Ue { get; set; }
        public string Descricao { get; set; }
        public string Instrucao { get; set; }
        public TipoPendencia TipoPendencia { get; set; }
    }

    public class SalvarPendenciaCalendarioUeCommandValidator : AbstractValidator<SalvarPendenciaCalendarioUeCommand>
    {
        public SalvarPendenciaCalendarioUeCommandValidator()
        {
            RuleFor(c => c.TipoCalendarioId)
            .NotEmpty()
            .WithMessage("O id do tipo de calendário deve ser informado para geração de pendência.");

            RuleFor(c => c.Ue)
            .NotEmpty()
            .WithMessage("A UE deve ser informada para geração de pendência.");

            RuleFor(c => c.Descricao)
            .NotEmpty()
            .WithMessage("A descrição deve ser informada para geração de pendência.");

            RuleFor(c => c.TipoPendencia)
            .NotEmpty()
            .WithMessage("O tipo de pendência deve ser informada para geração de pendência.");
        }

    }
}

using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;

namespace SME.SGP.Aplicacao
{
    public class ExcluirConsolidacoesReflexoFrequenciaBuscaAtivaUeMesCommand : IRequest<bool>
    {
        public ExcluirConsolidacoesReflexoFrequenciaBuscaAtivaUeMesCommand(string ueCodigo, int mes, int anoLetivo)
        {
            UeCodigo = ueCodigo;
            Mes = mes;
            AnoLetivo = anoLetivo;
        }

        public int Mes { get; set; }
        public int AnoLetivo { get; set; }
        public string UeCodigo { get; set; }
    }

    public class ExcluirConsolidacoesReflexoFrequenciaBuscaAtivaUeMesCommandValidator : AbstractValidator<ExcluirConsolidacoesReflexoFrequenciaBuscaAtivaUeMesCommand>
    {
        public ExcluirConsolidacoesReflexoFrequenciaBuscaAtivaUeMesCommandValidator()
        {
            RuleFor(c => c.Mes)
                .GreaterThanOrEqualTo(0)
                .LessThanOrEqualTo(12)
                .WithMessage("Um mês válido precisa ser informado");

            RuleFor(c => c.UeCodigo)
                .NotNull()
                .NotEmpty()
                .WithMessage("O código da Ue deve ser informado");

            RuleFor(c => c.AnoLetivo)
                .NotNull()
                .NotEmpty()
                .WithMessage("O ano letivo precisa ser informado");
        }
    }
}

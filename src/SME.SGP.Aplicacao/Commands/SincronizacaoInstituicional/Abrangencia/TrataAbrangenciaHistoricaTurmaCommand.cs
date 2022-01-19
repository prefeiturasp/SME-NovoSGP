using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class TrataAbrangenciaHistoricaTurmaCommand : IRequest<bool>
    {
        public TrataAbrangenciaHistoricaTurmaCommand(int anoLetivo, string codigoRf)
        {
            AnoLetivo = anoLetivo;

            ProfessorRf = codigoRf;
        }

        public int AnoLetivo { get; set; }

        public string ProfessorRf { get; set; }
    }

    public class TrataAbrangenciaHistoricaTurmaCommandValidator : AbstractValidator<TrataAbrangenciaHistoricaTurmaCommand>
    {
        public TrataAbrangenciaHistoricaTurmaCommandValidator()
        {
            RuleFor(c => c.AnoLetivo)
                .NotEmpty()
                .WithMessage("O Ano Letivo deve ser informado para sincronização.");

            RuleFor(c => c.ProfessorRf)
                .NotEmpty()
                .WithMessage("O Código RF do Professor deve ser informado para sincronização.");
        }
    }
}

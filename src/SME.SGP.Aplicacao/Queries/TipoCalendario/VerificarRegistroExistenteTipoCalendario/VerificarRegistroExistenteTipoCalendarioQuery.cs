using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class VerificarRegistroExistenteTipoCalendarioQuery : IRequest<bool>
    {
        public long TipoCalendarioId { get; set; }
        public string NomeTipoCalendario { get; set; }

        public VerificarRegistroExistenteTipoCalendarioQuery(long tipoCalendarioId,string nomeTipoCalendario)
        {
            TipoCalendarioId = tipoCalendarioId;
            NomeTipoCalendario = nomeTipoCalendario;
        }        
    }

    public class VerificarRegistroExistenteTipoCalendarioQueryValidator : AbstractValidator<VerificarRegistroExistenteTipoCalendarioQuery>
    {
        public VerificarRegistroExistenteTipoCalendarioQueryValidator()
        {
            RuleFor(x => x.NomeTipoCalendario)
                .NotEmpty()
                .WithMessage("O nome do tipo calendário deve ser informado.");
        }
    }
}
using FluentValidation;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class ObservacaoDiarioBordoDto
    {
        public string Observacao { get; set; }

        public IEnumerable<long> UsuariosIdNotificacao { get; set; }
    }

    public class ObservacaoDiarioBordoDtoValidator : AbstractValidator<ObservacaoDiarioBordoDto>
    {
        public ObservacaoDiarioBordoDtoValidator()
        {
            RuleFor(c => c.Observacao)
                .NotEmpty()
                .WithMessage("A observação deve ser informada.");
        }
    }
}

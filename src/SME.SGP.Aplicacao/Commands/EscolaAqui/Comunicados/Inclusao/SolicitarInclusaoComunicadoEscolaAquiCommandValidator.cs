using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class SolicitarInclusaoComunicadoEscolaAquiCommandValidator : AbstractValidator<SolicitarInclusaoComunicadoEscolaAquiCommand>
    {
        public SolicitarInclusaoComunicadoEscolaAquiCommandValidator()
        {
            RuleFor(c => c.Descricao)
                .NotEmpty()
                    .WithMessage("A anotação deve ser informada para atualização");

        }
    }



}

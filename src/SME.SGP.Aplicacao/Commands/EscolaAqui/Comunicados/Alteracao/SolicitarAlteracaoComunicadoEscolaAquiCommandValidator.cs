using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class SolicitarAlteracaoComunicadoEscolaAquiCommandValidator : AbstractValidator<SolicitarInclusaoComunicadoEscolaAquiCommand>
    {
        public SolicitarAlteracaoComunicadoEscolaAquiCommandValidator()
        {
            RuleFor(c => c.Descricao)
                .NotEmpty()
                    .WithMessage("A anotação deve ser informada para atualização");

        }
    }



}

using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class SolicitarInclusaoComunicadoEscolaAquiCommandValidator : AbstractValidator<SolicitarInclusaoComunicadoEscolaAquiCommand>
    {
        public SolicitarInclusaoComunicadoEscolaAquiCommandValidator()
        {
            RuleFor(x => x.DataEnvio).NotEmpty().WithMessage("A Data de envio do comunicado é obrigatória");
            RuleFor(x => x.DataExpiracao).NotEmpty().WithMessage("A Data de expiração do comunicado é obrigatória");
            RuleFor(x => x.Titulo).NotEmpty().WithMessage("O Título do comunicado é obrigatório");
            RuleFor(x => x.Descricao).NotEmpty().WithMessage("A Descrição do comunicado é obrigatória");
            RuleFor(x => x.Descricao).Length(5, 1024).WithMessage("A Mensagem do comunicado deve conter no máximo 1024 caracteres");
        }
    }
}

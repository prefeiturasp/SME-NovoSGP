using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class VerificaUsuarioPossuiArquivoQuery : IRequest<bool>
    {
        public VerificaUsuarioPossuiArquivoQuery(long tipoDocumentoId, long classificacaoId, long usuarioId, long ueId,long anoLetivo ,long documentoId = 0)
        {
            TipoDocumentoId = tipoDocumentoId;
            ClassificacaoId = classificacaoId;
            UsuarioId = usuarioId;
            UeId = ueId;
            DocumentoId = documentoId;
            AnoLetivo = anoLetivo;
        }

        public long DocumentoId { get; set; }
        public long TipoDocumentoId { get; set; }
        public long ClassificacaoId { get; set; }
        public long UsuarioId { get; set; }
        public long UeId { get; set; }
        public long AnoLetivo { get; set; }
    }

    public class VerificaUsuarioPossuiArquivoQueryValidator : AbstractValidator<VerificaUsuarioPossuiArquivoQuery>
    {
        public VerificaUsuarioPossuiArquivoQueryValidator()
        {
            RuleFor(c => c.TipoDocumentoId)
                .NotEmpty()
                .WithMessage("O id do tipo de documento deve ser informado.");

            RuleFor(c => c.ClassificacaoId)
                .NotEmpty()
                .WithMessage("O id da classificacao de documento deve ser informada.");
            
            RuleFor(c => c.UsuarioId)
                .NotEmpty()
                .WithMessage("O id do usuario deve ser informado.");

            RuleFor(c => c.UeId)
               .NotEmpty()
               .WithMessage("O id do UE deve ser informado.");            
            
            RuleFor(c => c.AnoLetivo)
               .NotEmpty()
               .WithMessage("O Ano Letivo deve ser informado.");
        }
    }
}

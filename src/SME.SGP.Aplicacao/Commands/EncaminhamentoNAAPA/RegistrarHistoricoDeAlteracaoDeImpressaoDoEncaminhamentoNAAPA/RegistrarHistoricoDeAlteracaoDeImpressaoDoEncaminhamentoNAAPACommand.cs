using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class RegistrarHistoricoDeAlteracaoDeImpressaoDoEncaminhamentoNAAPACommand : IRequest<bool>
    {
        public long[] EncaminhamentoNaapaIds { get; set; }

        public long UsuarioLogadoId { get; set; }

        public RegistrarHistoricoDeAlteracaoDeImpressaoDoEncaminhamentoNAAPACommand(long[] encaminhamentoNaapaIds, long usuarioLogadoId)
        {
            EncaminhamentoNaapaIds = encaminhamentoNaapaIds;
            UsuarioLogadoId = usuarioLogadoId;
        }
    }

    public class RegistrarHistoricoDeAlteracaoDeImpressaoDoEncaminhamentoNAAPACommandValidator : AbstractValidator<RegistrarHistoricoDeAlteracaoDeImpressaoDoEncaminhamentoNAAPACommand>
    {
        public RegistrarHistoricoDeAlteracaoDeImpressaoDoEncaminhamentoNAAPACommandValidator()
        {
            RuleFor(c => c.EncaminhamentoNaapaIds).NotEmpty().WithMessage("Os ids dos encaminhamentos NAAPA devem ser informados");
            RuleFor(c => c.UsuarioLogadoId).NotEmpty().WithMessage("O id do usuário logado deve ser informado");
        }
    }
}

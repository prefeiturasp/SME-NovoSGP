using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class RegistrarHistoricoDeAlteracaoDeImpressaoDoAtendimentoNAAPACommand : IRequest<bool>
    {
        public long[] EncaminhamentoNaapaIds { get; set; }

        public long UsuarioLogadoId { get; set; }

        public RegistrarHistoricoDeAlteracaoDeImpressaoDoAtendimentoNAAPACommand(long[] encaminhamentoNaapaIds, long usuarioLogadoId)
        {
            EncaminhamentoNaapaIds = encaminhamentoNaapaIds;
            UsuarioLogadoId = usuarioLogadoId;
        }
    }

    public class RegistrarHistoricoDeAlteracaoDeImpressaoDoAtendimentoNAAPACommandValidator : AbstractValidator<RegistrarHistoricoDeAlteracaoDeImpressaoDoAtendimentoNAAPACommand>
    {
        public RegistrarHistoricoDeAlteracaoDeImpressaoDoAtendimentoNAAPACommandValidator()
        {
            RuleFor(c => c.EncaminhamentoNaapaIds).NotEmpty().WithMessage("Os ids dos atendimentos NAAPA devem ser informados");
            RuleFor(c => c.UsuarioLogadoId).NotEmpty().WithMessage("O id do usuário logado deve ser informado");
        }
    }
}

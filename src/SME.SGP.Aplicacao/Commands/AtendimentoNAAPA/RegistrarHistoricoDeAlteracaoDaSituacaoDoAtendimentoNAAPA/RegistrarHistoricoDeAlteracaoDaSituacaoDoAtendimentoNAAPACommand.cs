using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;

namespace SME.SGP.Aplicacao
{
    public class RegistrarHistoricoDeAlteracaoDaSituacaoDoAtendimentoNAAPACommand : IRequest<long>
    {
        public EncaminhamentoNAAPA EncaminhamentoNAAPA { get; set; }
        public SituacaoNAAPA SituacaoAlterada { get; set; }

        public RegistrarHistoricoDeAlteracaoDaSituacaoDoAtendimentoNAAPACommand(EncaminhamentoNAAPA encaminhamentoNAAPA, SituacaoNAAPA situacaoAlterada)
        {
            this.EncaminhamentoNAAPA = encaminhamentoNAAPA;
            this.SituacaoAlterada = situacaoAlterada;
        }
    }

    public class RegistrarHistoricoDeAlteracaoDaSituacaoDoAtendimentoNAAPACommandValidator : AbstractValidator<RegistrarHistoricoDeAlteracaoDaSituacaoDoAtendimentoNAAPACommand>
    {
        public RegistrarHistoricoDeAlteracaoDaSituacaoDoAtendimentoNAAPACommandValidator()
        {
            RuleFor(c => c.EncaminhamentoNAAPA).NotEmpty().WithMessage("O atendimento NAAPA deve ser informado");
            RuleFor(c => c.SituacaoAlterada).NotEmpty().WithMessage("A situação deve ser informada");
        }
    }
}

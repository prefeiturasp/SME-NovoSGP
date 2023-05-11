using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;

namespace SME.SGP.Aplicacao
{
    public class RegistrarHistoricoDeAlteracaoDaSituacaoDoEncaminhamentoNAAPACommand : IRequest<long>
    {
        public EncaminhamentoNAAPA EncaminhamentoNAAPA { get; set; }
        public SituacaoNAAPA SituacaoAlterada { get; set; }

        public RegistrarHistoricoDeAlteracaoDaSituacaoDoEncaminhamentoNAAPACommand(EncaminhamentoNAAPA encaminhamentoNAAPA, SituacaoNAAPA situacaoAlterada)
        {
            this.EncaminhamentoNAAPA = encaminhamentoNAAPA;
            this.SituacaoAlterada = situacaoAlterada;
        }
    }

    public class RegistrarHistoricoDeAlteracaoDaSituacaoDoEncaminhamentoNAAPACommandValidator : AbstractValidator<RegistrarHistoricoDeAlteracaoDaSituacaoDoEncaminhamentoNAAPACommand>
    {
        public RegistrarHistoricoDeAlteracaoDaSituacaoDoEncaminhamentoNAAPACommandValidator()
        {
            RuleFor(c => c.EncaminhamentoNAAPA).NotEmpty().WithMessage("O encaminhamentos NAAPA deve ser informado");
            RuleFor(c => c.SituacaoAlterada).NotEmpty().WithMessage("A situação deve ser informada");
        }
    }
}

using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class RegistrarHistoricoDeAlteracaoEncaminhamentoNAAPACommand : IRequest<long>
    {
        public EncaminhamentoNAAPASecaoDto EncaminhamentoNAAPASecaoAlterado { get; set; }
        public EncaminhamentoNAAPA EncaminhamentoNAAPAExistente { get; set; }

        public RegistrarHistoricoDeAlteracaoEncaminhamentoNAAPACommand(EncaminhamentoNAAPASecaoDto encaminhamentoNAAPASecaoAlterado, EncaminhamentoNAAPA encaminhamentoNAAPAExistente)
        {
            EncaminhamentoNAAPASecaoAlterado = encaminhamentoNAAPASecaoAlterado;
            EncaminhamentoNAAPAExistente = encaminhamentoNAAPAExistente;
        }
    }

    public class RegistrarEncaminhamentoNAAPAHistoricoDeAlteracaoCommandValidator : AbstractValidator<RegistrarHistoricoDeAlteracaoEncaminhamentoNAAPACommand>
    {
        public RegistrarEncaminhamentoNAAPAHistoricoDeAlteracaoCommandValidator()
        {
            RuleFor(c => c.EncaminhamentoNAAPASecaoAlterado).NotEmpty().WithMessage("O encaminhamentos NAAPA da seção alterado deve ser informado");
            RuleFor(c => c.EncaminhamentoNAAPAExistente).NotEmpty().WithMessage("O encaminhamentos NAAPA atual deve ser informado");
        }
    }
}

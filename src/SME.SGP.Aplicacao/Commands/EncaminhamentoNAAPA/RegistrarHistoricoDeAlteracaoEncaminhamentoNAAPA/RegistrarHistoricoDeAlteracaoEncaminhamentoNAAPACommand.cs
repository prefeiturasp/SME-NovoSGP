using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class RegistrarHistoricoDeAlteracaoEncaminhamentoNAAPACommand : IRequest<long>
    {
        public EncaminhamentoNAAPASecaoDto EncaminhamentoNAAPASecaoAlterado { get; set; }
        public EncaminhamentoNAAPASecao EncaminhamentoNAAPASecaoExistente { get; set; }

        public RegistrarHistoricoDeAlteracaoEncaminhamentoNAAPACommand(EncaminhamentoNAAPASecaoDto encaminhamentoNAAPASecaoAlterado, EncaminhamentoNAAPASecao encaminhamentoNAAPASecaoExistente)
        {
            EncaminhamentoNAAPASecaoAlterado = encaminhamentoNAAPASecaoAlterado;
            EncaminhamentoNAAPASecaoExistente = encaminhamentoNAAPASecaoExistente;
        }
    }

    public class RegistrarEncaminhamentoNAAPAHistoricoDeAlteracaoCommandValidator : AbstractValidator<RegistrarHistoricoDeAlteracaoEncaminhamentoNAAPACommand>
    {
        public RegistrarEncaminhamentoNAAPAHistoricoDeAlteracaoCommandValidator()
        {
            RuleFor(c => c.EncaminhamentoNAAPASecaoAlterado).NotEmpty().WithMessage("O encaminhamentos NAAPA da seção alterado deve ser informado");
            RuleFor(c => c.EncaminhamentoNAAPASecaoExistente).NotEmpty().WithMessage("O encaminhamentos NAAPA da seção atual deve ser informado");
        }
    }
}

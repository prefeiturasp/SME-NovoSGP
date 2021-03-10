using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class RegistrarEncaminhamentoAEESecaoCommand : IRequest<EncaminhamentoAEESecao>
    {
        public long EncaminhamentoAEEId { get; set; }
        public long SecaoId { get; set; }
        public bool Concluido { get; set; }

        public RegistrarEncaminhamentoAEESecaoCommand(long encaminhamentoAEEId, long secaoId, bool concluido)
        {
            EncaminhamentoAEEId = encaminhamentoAEEId;
            SecaoId = secaoId;
            Concluido = concluido;
        }
    }
    public class RegistrarEncaminhamentoAEESecaoCommandCommandValidator : AbstractValidator<RegistrarEncaminhamentoAEESecaoCommand>
    {
        public RegistrarEncaminhamentoAEESecaoCommandCommandValidator()
        {
            RuleFor(x => x.EncaminhamentoAEEId)
                   .GreaterThan(0)
                   .WithMessage("O Id do Encaminhamento deve ser informado!");
            RuleFor(x => x.SecaoId)
                   .GreaterThan(0)
                   .WithMessage("O Id da Seção do Encaminhamento deve ser informada!");
        }
    }
}

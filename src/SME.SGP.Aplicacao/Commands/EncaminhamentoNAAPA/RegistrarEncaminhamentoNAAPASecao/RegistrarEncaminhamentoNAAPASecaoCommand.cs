using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class RegistrarEncaminhamentoNAAPASecaoCommand : IRequest<EncaminhamentoNAAPASecao>
    {
        public long EncaminhamentoNAAPAId { get; set; }
        public long SecaoId { get; set; }
        public bool Concluido { get; set; }

        public RegistrarEncaminhamentoNAAPASecaoCommand(long encaminhamentoNaapaId, long secaoId, bool concluido)
        {
            EncaminhamentoNAAPAId = encaminhamentoNaapaId;
            SecaoId = secaoId;
            Concluido = concluido;
        }
    }
    public class RegistrarEncaminhamentoNAAPASecaoCommandValidator : AbstractValidator<RegistrarEncaminhamentoNAAPASecaoCommand>
    {
        public RegistrarEncaminhamentoNAAPASecaoCommandValidator()
        {
            RuleFor(x => x.EncaminhamentoNAAPAId)
                   .GreaterThan(0)
                   .WithMessage("O Id do encaminhamento deve ser informado para registar Encaminhamento NAAPA!");
            RuleFor(x => x.SecaoId)
                   .GreaterThan(0)
                   .WithMessage("O Id da Seção do encaminhamento deve ser informada para registar Encaminhamento NAAPA!");
        }
    }
}

using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class RegistrarAtendimentoNAAPASecaoCommand : IRequest<EncaminhamentoNAAPASecao>
    {
        public long EncaminhamentoNAAPAId { get; set; }
        public long SecaoId { get; set; }
        public bool Concluido { get; set; }

        public RegistrarAtendimentoNAAPASecaoCommand(long encaminhamentoNaapaId, long secaoId, bool concluido)
        {
            EncaminhamentoNAAPAId = encaminhamentoNaapaId;
            SecaoId = secaoId;
            Concluido = concluido;
        }
    }
    public class RegistrarAtendimentoNAAPASecaoCommandValidator : AbstractValidator<RegistrarAtendimentoNAAPASecaoCommand>
    {
        public RegistrarAtendimentoNAAPASecaoCommandValidator()
        {
            RuleFor(x => x.EncaminhamentoNAAPAId)
                   .GreaterThan(0)
                   .WithMessage("O Id do atendimento deve ser informado para registar Atendimento NAAPA!");
            RuleFor(x => x.SecaoId)
                   .GreaterThan(0)
                   .WithMessage("O Id da Seção do atendimento deve ser informada para registar Atendimento NAAPA!");
        }
    }
}

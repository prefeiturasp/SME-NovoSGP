using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class RegistrarRegistroAcaoSecaoCommand : IRequest<RegistroAcaoBuscaAtivaSecao>
    {
        public long RegistroAcaoId { get; set; }
        public long SecaoId { get; set; }
        public bool Concluido { get; set; }

        public RegistrarRegistroAcaoSecaoCommand(long registroAcaoId, long secaoId, bool concluido)
        {
            RegistroAcaoId = registroAcaoId;
            SecaoId = secaoId;
            Concluido = concluido;
        }
    }
    public class RegistrarRegistroAcaoSecaoCommandValidator : AbstractValidator<RegistrarRegistroAcaoSecaoCommand>
    {
        public RegistrarRegistroAcaoSecaoCommandValidator()
        {
            RuleFor(x => x.RegistroAcaoId)
                   .GreaterThan(0)
                   .WithMessage("O Id do registro de ação deve ser informado para registar Registro Ação!");
            RuleFor(x => x.SecaoId)
                   .GreaterThan(0)
                   .WithMessage("O Id da Seção do registro de ação deve ser informada para registar Registro Ação!");
        }
    }
}

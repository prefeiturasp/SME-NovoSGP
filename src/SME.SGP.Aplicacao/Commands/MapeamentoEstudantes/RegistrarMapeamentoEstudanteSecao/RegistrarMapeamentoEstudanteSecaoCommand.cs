using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class RegistrarMapeamentoEstudanteSecaoCommand : IRequest<MapeamentoEstudanteSecao>
    {
        public long MapeamentoEstudanteId { get; set; }
        public long SecaoId { get; set; }
        public bool Concluido { get; set; }

        public RegistrarMapeamentoEstudanteSecaoCommand(long mapeamentoEstudanteId, long secaoId, bool concluido)
        {
            MapeamentoEstudanteId = mapeamentoEstudanteId;
            SecaoId = secaoId;
            Concluido = concluido;
        }
    }
    public class RegistrarMapeamentoEstudanteSecaoCommandValidator : AbstractValidator<RegistrarMapeamentoEstudanteSecaoCommand>
    {
        public RegistrarMapeamentoEstudanteSecaoCommandValidator()
        {
            RuleFor(x => x.MapeamentoEstudanteId)
                   .GreaterThan(0)
                   .WithMessage("O Id do mapeamento de estudante deve ser informado para registar a seção do mapeamento de estudante!");
            RuleFor(x => x.SecaoId)
                   .GreaterThan(0)
                   .WithMessage("O Id da Seção do registro de ação deve ser informada para registar a seção do mapeamento de estudante!");
        }
    }
}

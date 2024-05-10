using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class VerificaPendenciasFechamentoCommand : IRequest
    {
        public VerificaPendenciasFechamentoCommand(long fechamentoId,int bimestre, long turmaId)
        {
            FechamentoId = fechamentoId;
            Bimestre = bimestre;
            TurmaId = turmaId;
        }

        public long FechamentoId { get; set; }
        public int Bimestre { get; set; }
        public long TurmaId { get; set; }
    }

    public class VerificaPendenciasFechamentoCommandValidator : AbstractValidator<VerificaPendenciasFechamentoCommand>
    {
        public VerificaPendenciasFechamentoCommandValidator()
        {
            RuleFor(a => a.FechamentoId)
                .NotEmpty()
                .WithMessage("É necessário informar o indentificador do Fechamento para a verificação de pendências do fechamento");
            
            RuleFor(a => a.Bimestre)
                .NotEmpty()
                .WithMessage("É necessário informar o bimestre do Fechamento para a verificação de pendências do fechamento");
            
            RuleFor(a => a.TurmaId)
                .NotEmpty()
                .WithMessage("É necessário informar o indentificador da turma para a verificação de pendências do fechamento");
        }
    }
}

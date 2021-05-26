using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class SalvarHistoricoConceitoFechamentoCommand : IRequest<long>
    {
        public SalvarHistoricoConceitoFechamentoCommand(long? conceitoAnteriorId, long? conceitoNovoId, long fechamentoNotaId, string criadoRF = "", string criadoPor = "", long? workflowId = null)
        {
            ConceitoAnteriorId = conceitoAnteriorId;
            ConceitoNovoId = conceitoNovoId;
            FechamentoNotaId = fechamentoNotaId;
            WorkFlowId = workflowId;
        }

        public long? ConceitoAnteriorId { get; set; }
        public long? ConceitoNovoId { get; set; }
        public long FechamentoNotaId { get; set; }
        public string CriadoRF { get; set; }
        public string CriadoPor { get; set; }
        public long? WorkFlowId { get; set; }
    }

    public class SalvarHistoricoConceitoFechamentoCommandValidator : AbstractValidator<SalvarHistoricoConceitoFechamentoCommand>
    {
        public SalvarHistoricoConceitoFechamentoCommandValidator()
        {
            RuleFor(a => a.FechamentoNotaId)
            .NotEmpty()
            .WithMessage("O id da nota do fechamento deve ser informada para geração do histórico!");
        }
    }
}

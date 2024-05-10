using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class SalvarHistoricoNotaFechamentoCommand : IRequest<long>
    {
        public SalvarHistoricoNotaFechamentoCommand(double? notaAnterior, double? notaNova, long fechamentoNotaId, string criadoRF = "", string criadoPor = "", long? workFlowId = null)
        {
            NotaAnterior = notaAnterior;
            NotaNova = notaNova;
            FechamentoNotaId = fechamentoNotaId;
            CriadoRF = criadoRF;
            CriadoPor = criadoPor;
            WorkFlowId = workFlowId;
        }

        public double? NotaAnterior { get; set; }
        public double? NotaNova { get; set; }
        public long FechamentoNotaId { get; set; }
        public string CriadoRF { get; set; }
        public string CriadoPor { get; set; }
        public long? WorkFlowId { get; set; }
    }

    public class SalvarHistoricoNotaFechamentoCommandValidator : AbstractValidator<SalvarHistoricoNotaFechamentoCommand>
    {
        public SalvarHistoricoNotaFechamentoCommandValidator()
        {
            RuleFor(a => a.FechamentoNotaId)
            .NotEmpty()
            .WithMessage("O id da nota do fechamento deve ser informada para geração do histórico!");
        }
    }
}

using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class SalvarHistoricoNotaCommand : IRequest<long>
    {
        public SalvarHistoricoNotaCommand(double? notaAnterior, double? notaNova, long? conceitoAnteriorId, long? conceitoNovoId)
        {
            NotaAnterior = notaAnterior;
            NotaNova = notaNova;
            ConceitoAnteriorId = conceitoAnteriorId;
            ConceitoNovoId = conceitoNovoId;
        }

        public double? NotaAnterior { get; set; }
        public double? NotaNova { get; set; }
        public long? ConceitoAnteriorId { get; set; }
        public long? ConceitoNovoId { get; set; }
    }

    public class SalvarHistoricoNotaCommandValidator : AbstractValidator<SalvarHistoricoNotaCommand>
    {
        public SalvarHistoricoNotaCommandValidator()
        {
        }
    }
}

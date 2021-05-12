using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class SalvarHistoricoNotaCommand : IRequest<long>
    {
        public SalvarHistoricoNotaCommand(double? notaAnterior, double? notaNova, string criadoRF = "", string criadoPor = "")
        {
            NotaAnterior = notaAnterior;
            NotaNova = notaNova;
            CriadoRF = criadoRF;
            CriadoPor = criadoPor;
        }

        public double? NotaAnterior { get; set; }
        public double? NotaNova { get; set; }
        public string CriadoRF { get; set; }
        public string CriadoPor { get; set; }
    }

    public class SalvarHistoricoNotaCommandValidator : AbstractValidator<SalvarHistoricoNotaCommand>
    {
        public SalvarHistoricoNotaCommandValidator(){}
    }
}

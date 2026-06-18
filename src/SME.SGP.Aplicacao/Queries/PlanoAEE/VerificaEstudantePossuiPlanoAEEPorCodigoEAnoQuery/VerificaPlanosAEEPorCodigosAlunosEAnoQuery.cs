using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class VerificaPlanosAEEPorCodigosAlunosEAnoQuery : IRequest<IEnumerable<PlanoAEEResumoDto>>
    {
        public VerificaPlanosAEEPorCodigosAlunosEAnoQuery(string[] codigoEstudante, int anoLetivo)
        {
            CodigoEstudante = codigoEstudante;
            AnoLetivo = anoLetivo;
        }

        public string[] CodigoEstudante { get; }
        public int AnoLetivo { get; }
    }

    public class VerificaPlanosAEEPorCodigosAlunosEAnoQueryValidator : AbstractValidator<VerificaPlanosAEEPorCodigosAlunosEAnoQuery>
    {
        public VerificaPlanosAEEPorCodigosAlunosEAnoQueryValidator()
        {
            RuleFor(x => x.CodigoEstudante).NotNull().WithMessage("É preciso informar os códigos dos estudantes para consultar se possuir plano AEE.");
            RuleFor(x => x.AnoLetivo).GreaterThan(0).WithMessage("É preciso informar o ano letivo para consultar se possuir plano AEE.");
        }
    }
}

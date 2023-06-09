using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;

namespace SME.SGP.Aplicacao
{
    public class ObterFechamentoTurmaComConselhoDeClassePorTurmaCodigoSemestreTipoCalendarioQuery : IRequest<FechamentoTurma>
    {
        public ObterFechamentoTurmaComConselhoDeClassePorTurmaCodigoSemestreTipoCalendarioQuery(int bimestre, string codigoTurma, int anoLetivoTurma, int? semestre, long? tipoCalendario = null)
        {
            Bimestre = bimestre;
            CodigoTurma = codigoTurma;
            AnoLetivoTurma = anoLetivoTurma;
            Semestre = semestre;
            TipoCalendario = tipoCalendario;
        }

        public int Bimestre { get; set; }
        public string CodigoTurma { get; set; }
        public int AnoLetivoTurma { get; set; }
        public int? Semestre { get; set; }
        public long? TipoCalendario { get; set; }
    }

    public class ObterFechamentoTurmaComConselhoDeClassePorTurmaCodigoSemestreValidator : AbstractValidator<ObterFechamentoTurmaComConselhoDeClassePorTurmaCodigoSemestreTipoCalendarioQuery>
    {
        public ObterFechamentoTurmaComConselhoDeClassePorTurmaCodigoSemestreValidator()
        {
            RuleFor(c => c.CodigoTurma)
                .NotEmpty().WithMessage("Informe o Código da turma para consultar o fechamento da turma");
        }
    }
}
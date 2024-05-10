using System;
using System.Collections.Generic;
using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmaCodigosAlunoPorAnoLetivoUeTipoTurmaQuery : IRequest<string[]>
    {
        public ObterTurmaCodigosAlunoPorAnoLetivoUeTipoTurmaQuery(int anoLetivo, IEnumerable<int> tiposTurmas, bool consideraHistorico, string ueCodigo, int semestre,DateTime? dataReferencia)
        {
            AnoLetivo = anoLetivo;
            TiposTurmas = tiposTurmas;
            DataReferencia = dataReferencia;
            ConsideraHistorico = consideraHistorico;
            UeCodigo = ueCodigo;
            Semestre = semestre;
        }

        public int AnoLetivo { get; set; }
        public IEnumerable<int> TiposTurmas { get; set; }
        public DateTime? DataReferencia { get; }
        public bool ConsideraHistorico { get; set; }
        public string UeCodigo { get; set; }
        public int Semestre { get; set; }
    }

    public class ObterTurmaCodigosAlunoPorAnoLetivoUeTipoTurmaQueryValidator : AbstractValidator<ObterTurmaCodigosAlunoPorAnoLetivoUeTipoTurmaQuery>
    {
        public ObterTurmaCodigosAlunoPorAnoLetivoUeTipoTurmaQueryValidator()
        {
            RuleFor(a => a.AnoLetivo)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Necessário informar o ano letivo para obter o código da turma regular");
            RuleFor(a => a.UeCodigo)
                .NotEmpty()
                .WithMessage("Necessário informar o código da ue para obter o código da turma regular");
            
            RuleFor(a => a.Semestre)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Necessário informar o semestre para obter o código da turma regular");
        }
    }
}
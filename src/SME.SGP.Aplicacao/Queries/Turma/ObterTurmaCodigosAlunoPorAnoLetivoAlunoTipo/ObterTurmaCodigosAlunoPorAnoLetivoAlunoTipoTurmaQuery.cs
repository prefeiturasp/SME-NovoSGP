using FluentValidation;
using MediatR;
using SME.SGP.Dominio.Enumerados;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQuery : IRequest<string[]>
    {
        public ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQuery(int anoLetivo, string codigoAluno, IEnumerable<TipoTurma> tiposTurmas, bool? consideraHistorico = null, DateTime? dataReferencia = null)
        {
            AnoLetivo = anoLetivo;
            CodigoAluno = codigoAluno;
            TiposTurmas = tiposTurmas;
            DataReferencia = dataReferencia;
            ConsideraHistorico = VerificaConsideraHistorico(consideraHistorico);
        }
        public int AnoLetivo { get; set; }
        public string CodigoAluno { get; set; }
        public IEnumerable<TipoTurma> TiposTurmas { get; set; }
        public DateTime? DataReferencia { get; }
        public bool ConsideraHistorico { get; set; }

        private bool VerificaConsideraHistorico(bool? consideraHistorico)
        {
            if (consideraHistorico == null || !consideraHistorico.HasValue) return AnoLetivo == DateTime.Today.Year;
            else return consideraHistorico.Value;
        }
    }

    public class ObterCodigoTurmaRegularPorAnoLetivoAlunoQueryValidator : AbstractValidator<ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQuery>
    {
        public ObterCodigoTurmaRegularPorAnoLetivoAlunoQueryValidator()
        {
            RuleFor(a => a.AnoLetivo)
                .NotEmpty()
                .WithMessage("Necessário informar o ano letivo para obter o código da turma regular");
            RuleFor(a => a.CodigoAluno)
                .NotEmpty()
                .WithMessage("Necessário informar o código do aluno para obter o código da turma regular");
        }
    }
}

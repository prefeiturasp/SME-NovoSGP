using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciaAlunoPorAlunoTipoTurmasDisciplinasTurmasBimestresPeriodoEscolarQuery : IRequest<IEnumerable<FrequenciaAluno>>
    {
        public string CodigoAluno { get; set; }
        public TipoFrequenciaAluno TipoFrequencia { get; set; }
        public string[] DisciplinasId { get; set; }
        public string[] TurmasCodigo { get; set; }
        public int[] Bimestres { get; set; }
        public long[] periodosEscolaresId { get; set; }

        public ObterFrequenciaAlunoPorAlunoTipoTurmasDisciplinasTurmasBimestresPeriodoEscolarQuery(string codigoAluno, TipoFrequenciaAluno tipoFrequencia, string[] disciplinasId, string[] turmasCodigo, int[] bimestres, long[] periodosEscolaresId = null)
        {
            CodigoAluno = codigoAluno;
            TipoFrequencia = tipoFrequencia;
            DisciplinasId = disciplinasId;
            TurmasCodigo = turmasCodigo;
            Bimestres = bimestres;
            this.periodosEscolaresId = periodosEscolaresId;
        }
    }

    public class ObterFrequenciaAlunoPorAlunoTipoTurmasDisciplinasTurmasBimestresPeriodoEscolarQueryValidator : AbstractValidator<ObterFrequenciaAlunoPorAlunoTipoTurmasDisciplinasTurmasBimestresPeriodoEscolarQuery>
    {
        public ObterFrequenciaAlunoPorAlunoTipoTurmasDisciplinasTurmasBimestresPeriodoEscolarQueryValidator()
        {
            RuleFor(a => a.CodigoAluno)
                .NotEmpty()
                .WithMessage("O código do aluno deve ser informado para consulta de sua frequência aluno.");

            RuleFor(a => a.TipoFrequencia)
                .NotEmpty()
                .WithMessage("O tipo de frequência deve ser informado para consulta de sua frequência aluno.");
            
            RuleFor(a => a.DisciplinasId)
                .NotEmpty()
                .WithMessage("Os códigos das disciplinas devem ser informados para consulta de sua frequência aluno.");
            
            RuleFor(a => a.TurmasCodigo)
                .NotEmpty()
                .WithMessage("Os códigos das turmas devem ser informados para consulta de sua frequência aluno.");
            
            RuleFor(a => a.Bimestres)
                .NotEmpty()
                .WithMessage("Os bimestres devem ser informados para consulta de sua frequência aluno.");
        }
    }
}

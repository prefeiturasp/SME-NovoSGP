using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciaAlunoPorBimestreTurmaDisciplinaTipoQuery : IRequest<FrequenciaAluno>
    {
        public string CodigoAluno { get; set; }
        public int Bimestre { get; set; }
        public TipoFrequenciaAluno TipoFrequencia { get; set; }
        public string CodigoTurma { get; set; }
        public string DisciplinaId { get; set; }

        public ObterFrequenciaAlunoPorBimestreTurmaDisciplinaTipoQuery(string codigoAluno, int bimestre, TipoFrequenciaAluno tipoFrequencia, string codigoTurma, string disciplinaId = null)
        {
            CodigoAluno = codigoAluno;
            Bimestre = bimestre;
            TipoFrequencia = tipoFrequencia;
            CodigoTurma = codigoTurma;
            DisciplinaId = disciplinaId;
        }
    }

    public class ObterFrequenciaAlunoPorBimestreTurmaDisciplinaTipoQueryValidator : AbstractValidator<ObterFrequenciaAlunoPorBimestreTurmaDisciplinaTipoQuery>
    {
        public ObterFrequenciaAlunoPorBimestreTurmaDisciplinaTipoQueryValidator()
        {
            RuleFor(a => a.CodigoAluno)
                .NotEmpty()
                .WithMessage("É necessário informar o código do aluno para consulta das frequências.");
            RuleFor(a => a.CodigoAluno)
               .NotEmpty()
               .WithMessage("É necessário informar o bimestre para consulta das frequências.");
            RuleFor(a => a.CodigoAluno)
               .NotEmpty()
               .WithMessage("É necessário informar o código da turma para consulta das frequências.");
        }
    }
}

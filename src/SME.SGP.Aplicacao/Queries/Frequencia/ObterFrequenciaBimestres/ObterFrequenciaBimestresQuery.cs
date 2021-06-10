using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciaBimestresQuery : IRequest<IEnumerable<FrequenciaBimestreAlunoDto>>
    {
        public ObterFrequenciaBimestresQuery(string codigoAluno, int bimestre, string codigoTurma, TipoFrequenciaAluno tipoFrequencia)
        {
            CodigoAluno = codigoAluno;
            Bimestre = bimestre;
            CodigoTurma = codigoTurma;
            TipoFrequencia = tipoFrequencia;
        }

        public string CodigoAluno { get; set; }
        public int Bimestre { get; set; }
        public string CodigoTurma { get; set; }
        public TipoFrequenciaAluno TipoFrequencia { get; set; }
    }

    public class ObterFrequenciaBimestresQueryValidator : AbstractValidator<ObterFrequenciaBimestresQuery>
    {
        public ObterFrequenciaBimestresQueryValidator()
        {
            RuleFor(a => a.CodigoAluno)
                .NotEmpty()
                .WithMessage("Necessário informar o Código do Aluno");

            RuleFor(a => a.CodigoTurma)
                .NotEmpty()
                .WithMessage("Necessário informar o Código da Turma");
        }
    }
}

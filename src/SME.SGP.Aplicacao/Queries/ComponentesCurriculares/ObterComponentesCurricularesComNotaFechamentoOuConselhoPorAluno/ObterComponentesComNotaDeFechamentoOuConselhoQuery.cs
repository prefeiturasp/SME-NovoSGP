using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesComNotaDeFechamentoOuConselhoQuery : IRequest<IEnumerable<ComponenteCurricularDto>>
    {
        public ObterComponentesComNotaDeFechamentoOuConselhoQuery(int anoLetivo, long turmaId, int bimestre, string codigoAluno)
        {
            AnoLetivo = anoLetivo;
            TurmaId = turmaId;
            Bimestre = bimestre;
            CodigoAluno = codigoAluno;
        }

        public int AnoLetivo;
        public long TurmaId;
        public int Bimestre;
        public string CodigoAluno;


        public class ObterComponentesComNotaDeFechamentoOuConselhoQueryValidator : AbstractValidator<ObterComponentesComNotaDeFechamentoOuConselhoQuery>
        {
            public ObterComponentesComNotaDeFechamentoOuConselhoQueryValidator()
            {
                RuleFor(a => a.AnoLetivo)
                    .NotEmpty()
                    .WithMessage("Necessário informar o ano letivo para obter os componententes.");
                RuleFor(a => a.TurmaId)
                   .NotEmpty()
                   .WithMessage("Necessário informar o código do aluno para  obter os componententes.");
                RuleFor(a => a.Bimestre)
                    .NotEmpty()
                    .WithMessage("Necessário informar o  bimestre para  obter os componententes.");
                RuleFor(a => a.CodigoAluno)
                  .NotEmpty()
                  .WithMessage("Necessário informar o código do aluno para obter os componententes.");
            }
        }
    }
}


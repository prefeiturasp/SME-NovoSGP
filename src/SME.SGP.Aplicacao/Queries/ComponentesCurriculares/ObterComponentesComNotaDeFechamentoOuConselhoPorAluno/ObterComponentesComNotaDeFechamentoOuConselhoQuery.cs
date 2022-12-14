using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesComNotaDeFechamentoOuConselhoQuery : IRequest<IEnumerable<ComponenteCurricularDto>>
    {
        public ObterComponentesComNotaDeFechamentoOuConselhoQuery(int anoLetivo, string[] turmasId, int? bimestre, string codigoAluno)
        {
            AnoLetivo = anoLetivo;
            TurmasId = turmasId;
            Bimestre = bimestre;
            CodigoAluno = codigoAluno;
        }

        public int AnoLetivo;
        public string[] TurmasId;
        public int? Bimestre;
        public string CodigoAluno;


        public class ObterComponentesComNotaDeFechamentoOuConselhoQueryValidator : AbstractValidator<ObterComponentesComNotaDeFechamentoOuConselhoQuery>
        {
            public ObterComponentesComNotaDeFechamentoOuConselhoQueryValidator()
            {
                RuleFor(a => a.AnoLetivo)
                    .NotEmpty()
                    .WithMessage("Necessário informar o ano letivo para obter os componententes.");
                RuleFor(a => a.TurmasId)
                   .NotEmpty()
                   .WithMessage("Necessário informar o código do aluno para  obter os componententes.");                
                RuleFor(a => a.CodigoAluno)
                  .NotEmpty()
                  .WithMessage("Necessário informar o código do aluno para obter os componententes.");
            }
        }
    }
}
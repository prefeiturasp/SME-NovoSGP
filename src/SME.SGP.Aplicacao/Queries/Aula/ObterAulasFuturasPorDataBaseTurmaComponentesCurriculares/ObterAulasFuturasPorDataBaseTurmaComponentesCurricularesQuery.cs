using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterAulasFuturasPorDataBaseTurmaComponentesCurricularesQuery : IRequest<IEnumerable<AulaConsultaDto>>
    {
        public ObterAulasFuturasPorDataBaseTurmaComponentesCurricularesQuery(DateTime dataBase, string codigoTurma, string[] codigosComponentesCurriculares)
        {
            DataBase = dataBase;
            CodigoTurma = codigoTurma;
            CodigosComponentesCurriculares = codigosComponentesCurriculares;
        }

        public DateTime DataBase { get; private set; }
        public string CodigoTurma { get; private set; }
        public string[] CodigosComponentesCurriculares { get; private set; }
    }

    public class ObterAulasFuturasPorDataBaseTurmaComponentesCurricularesQueryValidator : AbstractValidator<ObterAulasFuturasPorDataBaseTurmaComponentesCurricularesQuery>
    {
        public ObterAulasFuturasPorDataBaseTurmaComponentesCurricularesQueryValidator()
        {
            RuleFor(c => c.DataBase)
                .NotEmpty()
                .WithMessage("A data base para busca de aulas futuras deve ser informada para a pesquisa de aulas.");

            RuleFor(c => c.CodigoTurma)
                .NotEmpty()
                .WithMessage("O código da turma deve ser informado para a pesquisa de aulas.");

            RuleFor(c => c.CodigosComponentesCurriculares)
                .NotEmpty()
                .WithMessage("Os códigos dos componentes curriculares devem ser informados para a pesquisa de aulas.");
        }
    }
}

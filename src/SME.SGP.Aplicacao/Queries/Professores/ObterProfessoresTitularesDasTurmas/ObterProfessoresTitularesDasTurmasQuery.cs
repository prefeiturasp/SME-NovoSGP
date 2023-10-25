using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterProfessoresTitularesDasTurmasQuery: IRequest<IEnumerable<ProfessorTitularDisciplinaEol>>
    {
        public ObterProfessoresTitularesDasTurmasQuery(IEnumerable<string> codigosTurmas)
        {
            CodigosTurmas = codigosTurmas;
        }

        public IEnumerable<string> CodigosTurmas { get; }
    }
    
    public class ObterProfessoresTitularesDasTurmasQueryValidator : AbstractValidator<ObterProfessoresTitularesDasTurmasQuery>
    {
        public ObterProfessoresTitularesDasTurmasQueryValidator()
        {
            RuleFor(a => a.CodigosTurmas)
                .NotNull()
                .WithMessage("Os códigos das turmas devem ser informados para obter os professores das turmas.");
        }
    }
}

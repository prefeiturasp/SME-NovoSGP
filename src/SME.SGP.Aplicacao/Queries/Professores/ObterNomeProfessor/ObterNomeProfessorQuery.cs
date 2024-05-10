using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterNomeProfessorQuery : IRequest<string>
    {
        public ObterNomeProfessorQuery(string rfProfessor)
        {
            RFProfessor = rfProfessor;
        }

        public string RFProfessor { get; set; }
    }
    public class ObterNomeProfessorQueryValidator : AbstractValidator<ObterNomeProfessorQuery>
    {
        public ObterNomeProfessorQueryValidator()
        {
            RuleFor(a => a.RFProfessor)
                .NotEmpty()
                .WithMessage("O RF do professor deve ser informado.");
        }
    }
}

using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterComponenteEhTerritorioQuery : IRequest<bool>
    {
        public long DisciplinaId { get; set; }
    }

    public class ObterComponenteEhTerritorioQueryValidator : AbstractValidator<ObterComponenteEhTerritorioQuery>
    {
        public ObterComponenteEhTerritorioQueryValidator()
        {
            RuleFor(a => a.DisciplinaId)
                .NotEmpty()
                .WithMessage("É necessário informar o id da disciplina para verificar se é território do saber.");
        }
    }
}

using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterUesComDrePorModalidadeTurmasQuery : IRequest<IEnumerable<Ue>>
    {
        public ObterUesComDrePorModalidadeTurmasQuery(Modalidade[] modalidade, int ano)
        {
            Modalidade = modalidade;
            Ano = ano;
        }

        public Modalidade[] Modalidade { get; set; }
        public int Ano { get; set; }
    }

    public class ObterUesComDrePorModalidadeTurmasQueryValidator : AbstractValidator<ObterUesComDrePorModalidadeTurmasQuery>
    {
        public ObterUesComDrePorModalidadeTurmasQueryValidator()
        {
            RuleFor(c => c.Ano)
               .Must(a => a > 0)
               .WithMessage("O ano letivo das turmas deve ser informado para consulta de UEs com turmas na modalidade.");

        }
    }
}

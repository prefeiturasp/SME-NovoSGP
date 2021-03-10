using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterModalidadeTurmaPorCodigoQuery : IRequest<Modalidade>
    {
        public ObterModalidadeTurmaPorCodigoQuery(string turmaCodigo)
        {
            TurmaCodigo = turmaCodigo;
        }

        public string TurmaCodigo { get; }
    }

    public class ObterModalidadeTurmaPorCodigoQueryValidator : AbstractValidator<ObterModalidadeTurmaPorCodigoQuery>
    {
        public ObterModalidadeTurmaPorCodigoQueryValidator()
        {
            RuleFor(a => a.TurmaCodigo)
                .NotEmpty()
                .WithMessage("Necessário informar o código da turma para obter sua Modalidade de ensino");
        }
    }
}

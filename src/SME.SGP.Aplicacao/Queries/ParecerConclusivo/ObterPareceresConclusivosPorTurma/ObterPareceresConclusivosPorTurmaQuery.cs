using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterPareceresConclusivosPorTurmaQuery: IRequest<IEnumerable<ParecerConclusivoDto>>
    {
        public ObterPareceresConclusivosPorTurmaQuery(string turmaCodigo, DateTime dataConsulta)
        {
            TurmaCodigo = turmaCodigo;
            DataConsulta = dataConsulta;
        }

        public string TurmaCodigo { get; set; }
        public DateTime DataConsulta { get; set; }
    }

    public class ObterPareceresConclusivosPorTurmaQueryValidator: AbstractValidator<ObterPareceresConclusivosPorTurmaQuery>
    {
        public ObterPareceresConclusivosPorTurmaQueryValidator()
        {
            RuleFor(c => c.TurmaCodigo)
                .NotEmpty()
                .WithMessage("O código da turma deve ser informado.");

            RuleFor(c => c.DataConsulta)
                .NotEmpty()
                .WithMessage("Necessário informar uma data para vigência dos pareceres.");
        }
    }
}

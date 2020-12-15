using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasPorUeModalidadesAnoQuery : IRequest<IEnumerable<Turma>>
    {
        public ObterTurmasPorUeModalidadesAnoQuery(long ueId, Modalidade[] modalidades, int ano)
        {
            UeId = ueId;
            Modalidades = modalidades;
            Ano = ano;
        }

        public long UeId { get; set; }
        public Modalidade[] Modalidades { get; set; }
        public int Ano { get; set; }
    }

    public class ObterTurmasPorUeModalidadeAnoQueryValidator : AbstractValidator<ObterTurmasPorUeModalidadesAnoQuery>
    {
        public ObterTurmasPorUeModalidadeAnoQueryValidator()
        {
            RuleFor(a => a.UeId)
               .Must(a => a > 0)
               .WithMessage("O id da UE deve ser informado para consulta de suas Turmas.");

            RuleFor(a => a.Modalidades)
               .Must(a => a.Length > 0)
               .WithMessage("As modalidades de turma devem ser informada para consulta de suas Turmas.");

            RuleFor(a => a.Ano)
               .Must(a => a > 0)
               .WithMessage("O ano deve ser informado para consulta de suas Turmas.");
        }
    }
}

﻿using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasComRegistrosIndividuaisPorModalidadeEAnoQuery : IRequest<IEnumerable<RegistroIndividualDTO>>
    {
        public ObterTurmasComRegistrosIndividuaisPorModalidadeEAnoQuery(int anoLetivo)
        {
            AnoLetivo = anoLetivo;
            Modalidades = new[] { (int)Modalidade.EducacaoInfantil };
        }

        public int AnoLetivo { get; set; }
        public int[] Modalidades { get; set; }
    }

    public class ObterTurmasComRegistrosIndividuaisPorModalidadeEAnoQueryValidator : AbstractValidator<ObterTurmasComRegistrosIndividuaisPorModalidadeEAnoQuery>
    {
        public ObterTurmasComRegistrosIndividuaisPorModalidadeEAnoQueryValidator()
        {
            RuleFor(c => c.AnoLetivo)
            .NotEmpty()
            .WithMessage("O Ano Letivo deve ser informado para consulta de turmas com registros individuais.");

        }
    }
}

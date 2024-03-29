﻿using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterTiposCalendariosPorAnosLetivoModalidadesQuery : IRequest<IEnumerable<TipoCalendarioBuscaDto>>
    {
        public ObterTiposCalendariosPorAnosLetivoModalidadesQuery(int[] anosLetivos, int[] modalidades, string descricao)
        {
            AnosLetivos = anosLetivos;
            Modalidades = modalidades;
            Descricao = descricao;
        }

        public int[] AnosLetivos { get; set; }

        public int[] Modalidades { get; set; }
        public string Descricao { get; }
    }

    public class ObterTiposCalendariosPorAnosLetivoModalidadesQueryValidator : AbstractValidator<ObterTiposCalendariosPorAnosLetivoModalidadesQuery>
    {
        public ObterTiposCalendariosPorAnosLetivoModalidadesQueryValidator()
        {
            RuleFor(x => x.Modalidades).NotEmpty().WithMessage("As modalidades são obrigatórias");
            RuleFor(x => x.AnosLetivos).NotEmpty().WithMessage("Os anos letivos são obrigatórios");
        }
    }
}

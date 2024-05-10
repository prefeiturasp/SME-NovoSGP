﻿using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries
{
    public class ObterSecoesEncaminhamentoNAAPAPorModalidadesQuery : IRequest<IEnumerable<SecaoQuestionarioDto>>
    {
        public ObterSecoesEncaminhamentoNAAPAPorModalidadesQuery(TipoQuestionario tipoQuestionario, int[] modalidades)
        {
            TipoQuestionario = tipoQuestionario;
            Modalidades = modalidades;
        }
        public int[] Modalidades { get; }
        public TipoQuestionario TipoQuestionario { get; }
    }
}

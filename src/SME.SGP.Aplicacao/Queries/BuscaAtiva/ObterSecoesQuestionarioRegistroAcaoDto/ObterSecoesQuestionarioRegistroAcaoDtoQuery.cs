﻿using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries
{
    public class ObterSecoesQuestionarioRegistroAcaoDtoQuery : IRequest<IEnumerable<SecaoQuestionarioDto>>
    {

        public ObterSecoesQuestionarioRegistroAcaoDtoQuery()
        {}

    }

}

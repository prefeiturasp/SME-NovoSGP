﻿using MediatR;
using SME.SGP.Infra.Dtos.EscolaAqui.DashboardAdesao;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterTotaisAdesaoAgrupadosPorDreQuery : IRequest<IEnumerable<TotaisAdesaoAgrupadoProDreResultado>>
    {
        public ObterTotaisAdesaoAgrupadosPorDreQuery()
        {}

        private static ObterTotaisAdesaoAgrupadosPorDreQuery _instance;
        public static ObterTotaisAdesaoAgrupadosPorDreQuery Instance => _instance ??= new();
    }
}

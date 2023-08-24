﻿using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterPlanosAEEAtivosComTurmaEVigenciaQuery : IRequest<IEnumerable<PlanoAEEReduzidoDto>>
    {
        public ObterPlanosAEEAtivosComTurmaEVigenciaQuery()
        {}

        private static ObterPlanosAEEAtivosComTurmaEVigenciaQuery _instance;
        public static ObterPlanosAEEAtivosComTurmaEVigenciaQuery Instance => _instance ??= new();
    }
}

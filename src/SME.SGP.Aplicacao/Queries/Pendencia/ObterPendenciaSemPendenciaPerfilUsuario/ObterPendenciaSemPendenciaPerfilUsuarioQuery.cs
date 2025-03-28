﻿using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciaSemPendenciaPerfilUsuarioQuery : IRequest<IEnumerable<PendenciaPendenteDto>>
    {
        private static ObterPendenciaSemPendenciaPerfilUsuarioQuery _instance;
        public static ObterPendenciaSemPendenciaPerfilUsuarioQuery Instance => _instance ??= new();
    }
}

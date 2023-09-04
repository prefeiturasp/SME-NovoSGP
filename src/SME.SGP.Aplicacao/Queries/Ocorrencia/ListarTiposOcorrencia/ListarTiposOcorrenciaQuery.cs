using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ListarTiposOcorrenciaQuery : IRequest<IEnumerable<OcorrenciaTipoDto>>
    {
        private static ListarTiposOcorrenciaQuery _instance;
        public static ListarTiposOcorrenciaQuery Instance => _instance ??= new();
    }
}

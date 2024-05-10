using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciaCargaQuery : IRequest<IEnumerable<PendenciaPendenteDto>>
    {
        public ObterPendenciaCargaQuery()
        {}

        private static ObterPendenciaCargaQuery _instance;
        public static ObterPendenciaCargaQuery Instance => _instance ??= new();
    }
}

using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterTipoDocumentoClassificacaoQuery : IRequest<IEnumerable<TipoDocumentoDto>>
    {
        public ObterTipoDocumentoClassificacaoQuery()
        { }

        private static ObterTipoDocumentoClassificacaoQuery _instance;
        public static ObterTipoDocumentoClassificacaoQuery Instance => _instance ??= new();
    }

}

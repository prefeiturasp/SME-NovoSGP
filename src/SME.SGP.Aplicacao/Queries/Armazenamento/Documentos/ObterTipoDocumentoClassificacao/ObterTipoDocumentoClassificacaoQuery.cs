using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterTipoDocumentoClassificacaoQuery : IRequest<IEnumerable<TipoDocumentoDto>>
    {
        public ObterTipoDocumentoClassificacaoQuery()
        {}

        private static ObterTipoDocumentoClassificacaoQuery _instance;
        public static ObterTipoDocumentoClassificacaoQuery Instance => _instance ??= new();
    }

}

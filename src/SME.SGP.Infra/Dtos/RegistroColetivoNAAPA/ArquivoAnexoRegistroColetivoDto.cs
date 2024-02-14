using SME.SGP.Dominio;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class ArquivoAnexoRegistroColetivoDto
    {
        public long Id { get; set; }
        public Guid Codigo { get; set; }
        public string Nome { get; set; }
    }
}

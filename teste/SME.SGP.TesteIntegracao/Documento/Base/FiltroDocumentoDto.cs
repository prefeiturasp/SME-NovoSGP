using System;
using SME.SGP.Dominio;

namespace SME.SGP.TesteIntegracao.Documento
{
    public class FiltroDocumentoDto
    {
        public string Perfil { get; set; }
        public Modalidade Modalidade { get; set; }
        public ModalidadeTipoCalendario TipoCalendario { get; set; }
    }
} 
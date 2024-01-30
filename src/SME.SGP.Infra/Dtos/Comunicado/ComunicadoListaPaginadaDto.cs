using Newtonsoft.Json.Linq;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SME.SGP.Infra
{
    public class ComunicadoListaPaginadaDto
    {
        public long Id { get; set; }
        public string Titulo { get; set; }
        public DateTime DataEnvio { get; set; }
        public DateTime DataExpiracao { get; set; }
        public int[] ModalidadeCodigo { get; set; }
        public int[] TipoEscolaCodigo { get; set; }
    }
}

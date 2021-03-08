using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Dto
{
    public class ComunicadoAlunoReduzidoDto
    {
        public long ComunicadoId { get; set; }
        public DateTime DataEnvio { get; set; }
        public string Titulo { get; set; }
        public TipoComunicado Categoria { get; set; }
        public string CategoriaNome { get; set; }
        public string StatusLeitura { get; set; }
    }
}
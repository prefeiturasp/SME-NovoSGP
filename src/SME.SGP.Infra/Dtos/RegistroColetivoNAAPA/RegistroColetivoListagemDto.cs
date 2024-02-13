using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using System;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class RegistroColetivoListagemDto
    {
        public RegistroColetivoListagemDto()
        {
        }
        public long Id { get; set; }
        public string[] NomesUe { get; set; }
        public DateTime DataReuniao { get; set; }
        public string TipoReuniaoDescricao { get; set; }
        public string CriadoPor { get; set; }

    }
}

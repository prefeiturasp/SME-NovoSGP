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
            NomesUe = new List<string>();
        }
        public long Id { get; set; }
        public List<string> NomesUe { get; }
        public DateTime DataReuniao { get; set; }
        public string TipoReuniaoDescricao { get; set; }
        public string NomeUsuarioCriador { get; set; }
        public string RfUsuarioCriador { get; set; }
        public string CriadoPor => $"{NomeUsuarioCriador}({RfUsuarioCriador})";
    }
}

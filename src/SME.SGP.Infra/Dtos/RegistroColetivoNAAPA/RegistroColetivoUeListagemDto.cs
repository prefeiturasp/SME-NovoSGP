using SME.SGP.Dominio;
using System;

namespace SME.SGP.Infra
{
    public class RegistroColetivoUeListagemDto
    {
        public RegistroColetivoUeListagemDto()
        {
        }
        public long Id { get; set; }
        public string NomeUe { get; set; }
        public DateTime DataReuniao { get; set; }
        public string TipoReuniaoDescricao { get; set; }
        public TipoEscola TipoEscola { get; set; }
        public string NomeUsuarioCriador { get; set; }
        public string RfUsuarioCriador { get; set; }
        public string NomeTipoUe => TipoEscola != TipoEscola.Nenhum ? $"{TipoEscola.ObterNomeCurto()} {NomeUe}" : $"{NomeUe}";
        public string CriadoPor => $"{NomeUsuarioCriador}({RfUsuarioCriador})";

    }
}

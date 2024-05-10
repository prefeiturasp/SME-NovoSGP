using SME.SGP.Dominio;

namespace SME.SGP.Dto
{
    public class FiltroEventoTipoDto
    {
        public string Descricao { get; set; }
        public bool EhCadastro { get; set; }
        public EventoLetivo Letivo { get; set; }
        public EventoLocalOcorrencia LocalOcorrencia { get; set; }
    }
}
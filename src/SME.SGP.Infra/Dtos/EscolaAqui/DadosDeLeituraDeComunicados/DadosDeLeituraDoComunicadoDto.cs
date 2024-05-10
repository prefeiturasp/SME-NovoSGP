namespace SME.SGP.Infra.Dtos.EscolaAqui.DadosDeLeituraDeComunicados
{
    public class DadosDeLeituraDoComunicadoDto
    {
        public string NomeAbreviadoDre { get; set; }
        public string NomeAbreviadoUe { get; set; }
        public long NaoReceberamComunicado { get; set; }
        public long ReceberamENaoVisualizaram { get; set; }
        public long VisualizaramComunicado { get; set; }
    }
}
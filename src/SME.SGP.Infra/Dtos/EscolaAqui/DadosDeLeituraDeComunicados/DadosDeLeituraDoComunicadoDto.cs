namespace SME.SGP.Infra.Dtos.EscolaAqui.DadosDeLeituraDeComunicados
{
    public class DadosDeLeituraDoComunicadoDto
    {
        public string NomeCompletoDre { get; set; }
        public string NomeCompletoUe { get; set; }
        public long NaoReceberamComunicado { get; set; }
        public long ReceberamENaoVisualizaram { get; set; }
        public long VisualizaramComunicado { get; set; }
    }
}
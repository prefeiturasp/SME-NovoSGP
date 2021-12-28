namespace SME.SGP.Infra
{
    public class NotasConceitosNotaAvaliacaoListaoRetornoDto
    {
        public NotasConceitosNotaAvaliacaoListaoRetornoDto()
        {
            PodeEditar = true;
        }

        public long AtividadeAvaliativaId { get; set; }
        public bool Ausente { get; set; }
        public string NotaConceito { get; set; }
        public bool PodeEditar { get; set; }
    }
}

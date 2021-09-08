using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class NotasConceitosNotaAvaliacaoRetornoDto
    {
        public NotasConceitosNotaAvaliacaoRetornoDto()
        {
            PodeEditar = true;
        }

        public long AtividadeAvaliativaId { get; set; }
        public bool Ausente { get; set; }
        public string NotaConceito { get; set; }
        public bool PodeEditar { get; set; }
        public StatusGSA StatusGsa { get; set; }
    }
}
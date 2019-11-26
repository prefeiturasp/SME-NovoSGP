using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class AtribuicaoCJListaTitularesFiltroDto
    {
        [EnumeradoRequirido(ErrorMessage = "É necessário informar a modalidade.")]
        public Modalidade Modalidade { get; set; }

        public string TurmaId { get; set; }
        public string UeId { get; set; }
        public string UsuarioRf { get; set; }
    }
}
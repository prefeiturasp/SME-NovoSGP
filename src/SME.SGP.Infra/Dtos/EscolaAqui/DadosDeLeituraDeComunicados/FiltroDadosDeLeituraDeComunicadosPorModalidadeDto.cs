using SME.SGP.Dominio;
using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra.Dtos.EscolaAqui.DadosDeLeituraDeComunicados
{
    public class FiltroDadosDeLeituraDeComunicadosPorModalidadeDto
    {
        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }
        [Required(ErrorMessage = "O comunicado deve ser informado.")]
        public long NotificacaoId { get; set; }
        public Modalidade Modalidade { get; set; }
        [Required(ErrorMessage = "O modo de visualização deve ser informado.")]
        public int ModoVisualizacao { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra.Dtos.EscolaAqui.DadosDeLeituraDeComunicados
{
    public class ObterDadosDeLeituraDeComunicadosAgrupadosPorDreDto
    {
        [Required(ErrorMessage = "O comunicado deve ser informado.")]
        public long NotificacaoId { get; set; }
        [Required(ErrorMessage = "O modo de visualização deve ser informado.")]
        public int ModoVisualizacao { get; set; }
    }
}

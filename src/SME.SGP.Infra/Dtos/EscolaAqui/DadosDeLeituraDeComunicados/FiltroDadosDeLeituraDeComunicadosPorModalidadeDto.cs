using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra.Dtos.EscolaAqui.DadosDeLeituraDeComunicados
{
    public class FiltroDadosDeLeituraDeComunicadosPorModalidadeDto
    {
        [Required(ErrorMessage = "O código da DRE deve ser informado.")]
        public string CodigoDre { get; set; }

        [Required(ErrorMessage = "O código da UE deve ser informado.")]
        public string CodigoUe { get; set; }

        [Required(ErrorMessage = "O comunicado deve ser informado.")]
        public long NotificacaoId { get; set; }

        public short[] Modalidades { get; set; }

        public long[] CodigosTurmas { get; set; }

        [Required(ErrorMessage = "O modo de visualização deve ser informado.")]
        public int ModoVisualizacao { get; set; }
    }

    public class FiltroDadosDeLeituraDeComunicadosPorAlunosTurmaDto
    {
        [Required(ErrorMessage = "O comunicado deve ser informado.")]
        public long ComunicadoId { get; set; }

        [Required(ErrorMessage = "O código da turma deve ser informado.")]
        public long CodigoTurma { get; set; }
    }
}

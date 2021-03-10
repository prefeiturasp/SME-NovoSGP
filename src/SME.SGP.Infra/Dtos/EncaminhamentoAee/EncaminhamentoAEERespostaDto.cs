using SME.SGP.Dominio.Enumerados;

namespace SME.SGP.Infra.Dtos
{
    public class EncaminhamentoAEERespostaDto
    {
        public SituacaoAEE Situacao { get; set; }
        public string SituacaoDescricao { get; set; }
        public string MotivoEncerramento { get; set; }
        public bool PodeEditar { get; set; }
        public bool PodeAtribuirResponsavel { get; set; }
        public AlunoReduzidoDto Aluno { get; set; }
        public TurmaAnoDto Turma { get; set; }
        public ResponsavelEncaminhamentoAEEDto responsavelEncaminhamentoAEE { get; set; }
    }
}

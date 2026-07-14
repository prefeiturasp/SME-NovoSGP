using SME.SGP.Dominio.Enumerados;

namespace SME.SGP.Infra
{
    public class PlanoAEETurmaDto
    {
        public long Id { get; set; }
        public long TurmaId { get; set; }
        public SituacaoPlanoAEE Situacao { get; set; }
        public string AlunoNome { get; set; }
        public string AlunoCodigo { get; set; }

    }

}

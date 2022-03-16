using Microsoft.AspNetCore.Http;

namespace SME.SGP.Infra
{
    public class AcompanhamentoAlunoDto
    {
        public long AcompanhamentoAlunoId { get; set; }
        public long AcompanhamentoAlunoSemestreId { get; set; }
        public long TurmaId { get; set; }
        public int Semestre { get; set; }
        public string AlunoCodigo { get; set; }
        public string Observacoes { get; set; }
        public string PercursoIndividual { get; set; }
        public bool TextoSugerido { get; set; }
        public IFormFile File { get; set; }
    }
}

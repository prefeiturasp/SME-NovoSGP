namespace SME.SGP.Infra.Dtos.PlanoAEE
{
    public class PlanoAEEResumoIntegracaoDto
    {
        public long Id { get; set; }
        public string Numero { get; set; }
        public string Nome { get; set; }
        public string Turma { get; set; }
        public int Situacao { get; set; }
        public string CodigoAluno { get; set; }
    }
}
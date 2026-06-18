namespace SME.SGP.Infra
{
    public class FechamentoAlunoCompletoDto: AuditoriaDto
    {
        public AlunoDadosBasicosDto Aluno { get; set; }
        public string Anotacao { get; set; }
    }
}

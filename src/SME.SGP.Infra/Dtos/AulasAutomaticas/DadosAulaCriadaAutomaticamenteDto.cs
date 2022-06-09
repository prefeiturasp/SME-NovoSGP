namespace SME.SGP.Infra
{
    public class DadosAulaCriadaAutomaticamenteDto
    {
        public DadosAulaCriadaAutomaticamenteDto((string codigo, string nome) dadosDisciplina, int quantidadeAulas = 1, string rfProfessor = "Sistema")
        {
            ComponenteCurricular = dadosDisciplina;
            QuantidadeAulas = quantidadeAulas;
            RfProfessor = rfProfessor;
        }

        public (string codigo, string nome) ComponenteCurricular { get; set; }
        public int QuantidadeAulas { get; set; }
        public string RfProfessor { get; set; }
    }
}

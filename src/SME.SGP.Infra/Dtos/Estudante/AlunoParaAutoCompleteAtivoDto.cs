namespace SME.SGP.Infra.Dtos
{
    public class AlunoParaAutoCompleteAtivoDto
    {
        public long CodigoAluno { get; set; }
        public string NomeAluno { get; set; }
        public string NomeSocialAluno { get; set; }
        public long CodigoTurma { get; set; }
        public string NumeroAlunoChamada { get; set; }
        public string Turma { get; set; }
        public string Modalidade { get; set; }
        public string NomeFinal
        {
            get
            {
                if (!string.IsNullOrEmpty(NomeSocialAluno))
                    return NomeSocialAluno;
                else return NomeAluno;
            }
        }

        public string NomeAlunoComTurmaModalidade { get { return $"{NomeFinal} - {Modalidade} - {Turma}"; } }
    }
}

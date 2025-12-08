namespace SME.SGP.Infra
{
    public class RespostaTurmaProgramaAtendimentoNAAPADto
    {
        public string dreUe { get; set; }
        public string turma { get; set; }
        public string componenteCurricular { get; set; }
        
        public bool EhIgual(object o)
        {
            if (!(o is RespostaTurmaProgramaAtendimentoNAAPADto)) return false;

            RespostaTurmaProgramaAtendimentoNAAPADto resposta = (RespostaTurmaProgramaAtendimentoNAAPADto)o;
            return (this.dreUe == resposta.dreUe
                && this.turma == resposta.turma
                && this.componenteCurricular == resposta.componenteCurricular
                );

        }
    }
}

namespace SME.SGP.Infra
{
    public class RespostaTurmaProgramaEncaminhamentoNAAPADto
    {
        public string dreUe { get; set; }
        public string turma { get; set; }
        public string componenteCurricular { get; set; }
        
        public bool EhIgual(object o)
        {
            if (!(o is RespostaTurmaProgramaEncaminhamentoNAAPADto)) return false;

            RespostaTurmaProgramaEncaminhamentoNAAPADto resposta = (RespostaTurmaProgramaEncaminhamentoNAAPADto)o;
            return (this.dreUe == resposta.dreUe
                && this.turma == resposta.turma
                && this.componenteCurricular == resposta.componenteCurricular
                );

        }
    }
}

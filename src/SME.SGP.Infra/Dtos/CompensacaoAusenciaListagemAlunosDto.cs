namespace SME.SGP.Infra
{
    public class CompensacaoAusenciaListagemAlunosDto
    {
        public CompensacaoAusenciaListagemAlunosDto(string nome, bool ehMatriculadoTurmaPap = false)
        {
            Nome = nome;
            EhMatriculadoTurmaPAP = ehMatriculadoTurmaPap;
        }

        public string Nome { get; set; }
        public bool EhMatriculadoTurmaPAP { get; set; }
    }
}
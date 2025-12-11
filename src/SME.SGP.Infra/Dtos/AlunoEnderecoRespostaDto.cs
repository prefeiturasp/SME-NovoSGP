namespace SME.SGP.Dto
{
    public class AlunoEnderecoRespostaDto
    {
        public string CodigoAluno { get; set; }
        public string NomeAluno { get; set; }
        public string NomeMae { get; set; }
        public string Sexo { get; set; }
        public string GrupoEtnico { get; set; }
        public string Nacionalidade { get; set; }
        public string NacionalidadeResponsavel { get; set; }
        public EnderecoRespostaDto Endereco { get; set; }
        public bool EhImigrante { get; set; }
        public bool ResponsavelEhImigrante { get; set; }
        public string NIS { get; set; }
        public string CNS { get; set; }
        public string TEG { get; set; }
    }
}

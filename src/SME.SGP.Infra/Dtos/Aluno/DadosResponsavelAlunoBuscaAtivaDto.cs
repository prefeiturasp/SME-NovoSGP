namespace SME.SGP.Infra
{ 
    public class DadosResponsavelAlunoBuscaAtivaDto
    {
        public DadosResponsavelAlunoBuscaAtivaDto(DadosResponsavelAlunoEolDto dadosResponsavel, AtualizarDadosUsuarioDto atualizarDados)
        {
            CodigoAluno = dadosResponsavel.CodigoAluno;
            Cpf = dadosResponsavel.Cpf;
            Email = atualizarDados.Email;
            DDDCelular = atualizarDados.DDDCelular;
            NumeroCelular = atualizarDados.NumeroCelular;
            DDDComercial = atualizarDados.DDDComercial;
            NumeroComercial = atualizarDados.NumeroComercial;
            DDDResidencial = atualizarDados.DDDResidencial;
            NumeroResidencial = atualizarDados.NumeroResidencial;
        }
        public string CodigoAluno { get; set; }
        public string Cpf { get; set; }
        public string Email { get; set; }
        public string DDDCelular { get; set; }
        public string NumeroCelular { get; set; }
        public string DDDResidencial { get; set; }
        public string NumeroResidencial { get; set; }
        public string DDDComercial { get; set; }
        public string NumeroComercial { get; set; }
    }
}

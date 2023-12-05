using SME.SGP.Dominio;
using SME.SGP.Dto;

namespace SME.SGP.Infra.Dtos.Aluno
{
    public class DadosResponsavelAlunoDto
    {
        public string CodigoAluno { get; set; }
        public string NomeResponsavel { get; set; }
        public string Cpf { get; set; }
        public string Email { get; set; }
        public string DDDCelular { get; set; }
        public string NumeroCelular { get; set; }
        public string DDDResidencial { get; set; }
        public string NumeroResidencial { get; set; }
        public string DDDComercial { get; set; }
        public string NumeroComercial { get; set; }
        public TipoResponsavel TipoResponsavel { get; set; }
        public EnderecoRespostaDto Endereco { get; set; }
    }
}

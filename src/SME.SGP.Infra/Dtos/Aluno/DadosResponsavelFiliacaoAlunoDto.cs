using SME.SGP.Dto;
using System.Collections.Generic;

namespace SME.SGP.Infra.Dtos.Aluno
{
    public class DadosResponsavelFiliacaoAlunoDto
    {
        public string CodigoAluno { get; set; }
        public string Cpf { get; set; }
        public string Email { get; set; }
        public string NomeFiliacao1 { get; set; }
        public List<TelefonesDto> TelefonesFiliacao1 { get; set; }
        public string NomeFiliacao2 { get; set; }
        public List<TelefonesDto> TelefonesFiliacao2 { get; set; }
        public EnderecoRespostaDto Endereco { get; set; }
    }
}

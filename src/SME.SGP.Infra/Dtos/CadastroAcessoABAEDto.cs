using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public class CadastroAcessoABAEDto
    {
        public long Id { get; set; }
        
        [Required(ErrorMessage = "É necessário informar o identificador da UE para o cadastro de acesso ABAE")]
        public long UeId { get; set; }
        
        [Required(ErrorMessage = "É necessário informar o nome do usuáro para o cadastro de acesso ABAE")]
        [MaxLength(100, ErrorMessage = "O nome do usuário não pode conter mais que 100 caracteres")]
        public string Nome { get; set; }
        
        [Required(ErrorMessage = "É necessário informar o cpf do usuário para o cadastro de acesso ABAE")]
        public string Cpf { get; set; }
        
        [Required(ErrorMessage = "É necessário informar o e-mail do usuário para o cadastro de acesso ABAE")]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "É necessário informar o telefone do usuário para o cadastro de acesso ABAE")]
        public string Telefone { get; set; }
        
        public bool Situacao { get; set; }
        
        [Required(ErrorMessage = "É necessário informar o cep do usuário para o cadastro de acesso ABAE")]
        public string Cep { get; set; }
        
        [Required(ErrorMessage = "É necessário informar o endereço do usuário para o cadastro de acesso ABAE")]
        public string Endereco { get; set; }
        
        [Required(ErrorMessage = "É necessário informar o número do usuário para o cadastro de acesso ABAE")]
        public int Numero { get; set; }
        
        [Required(ErrorMessage = "É necessário informar o complemento do usuário para o cadastro de acesso ABAE")]
        public string Complemento { get; set; }
        
        [Required(ErrorMessage = "É necessário informar o cidade do usuário para o cadastro de acesso ABAE")]
        public string Cidade { get; set; }
        
        [Required(ErrorMessage = "É necessário informar o estado do usuário para o cadastro de acesso ABAE")]
        public string Estado { get; set; }
        
        public bool Excluido { get; set; }
    }
}

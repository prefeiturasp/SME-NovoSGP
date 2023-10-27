using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public class CadastroAcessoABAEDto : AuditoriaDto
    {
        [Required(ErrorMessage = "É necessário informar o identificador da UE para o cadastro de acesso ABAE")]
        public long UeId { get; set; }
        
        [Required(ErrorMessage = "É necessário informar o nome do usuáro para o cadastro de acesso ABAE")]
        [MaxLength(100, ErrorMessage = "O nome do usuário não pode conter mais que 100 caracteres")]
        public string Nome { get; set; }
        
        [Required(ErrorMessage = "É necessário informar o cpf do usuário para o cadastro de acesso ABAE")]
        [MaxLength(15, ErrorMessage = "O cpf do usuário não pode conter mais que 15 caracteres")]
        public string Cpf { get; set; }
        
        [Required(ErrorMessage = "É necessário informar o e-mail do usuário para o cadastro de acesso ABAE")]
        [MaxLength(80, ErrorMessage = "O e-mail do usuário não pode conter mais que 80 caracteres")]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "É necessário informar o telefone do usuário para o cadastro de acesso ABAE")]
        [MaxLength(15, ErrorMessage = "O telefone do usuário não pode conter mais que 15 caracteres")]
        public string Telefone { get; set; }
        
        public bool Situacao { get; set; }
        
        [Required(ErrorMessage = "É necessário informar o cep do usuário para o cadastro de acesso ABAE")]
        [MaxLength(10, ErrorMessage = "O cep do usuário não pode conter mais que 10 caracteres")]
        public string Cep { get; set; }
        
        [Required(ErrorMessage = "É necessário informar o endereço do usuário para o cadastro de acesso ABAE")]
        [MaxLength(200, ErrorMessage = "O endereço do usuário não pode conter mais que 200 caracteres")]
        public string Endereco { get; set; }
        
        [Required(ErrorMessage = "É necessário informar o número do usuário para o cadastro de acesso ABAE")]
        public int Numero { get; set; }
        
        [MaxLength(20, ErrorMessage = "O complemento do usuário não pode conter mais que 20 caracteres")]
        public string? Complemento { get; set; }
        
        [Required(ErrorMessage = "É necessário informar o bairro do usuário para o cadastro de acesso ABAE")]
        [MaxLength(50, ErrorMessage = "O bairro do usuário não pode conter mais que 50 caracteres")]
        public string Bairro { get; set; }
        
        [Required(ErrorMessage = "É necessário informar o cidade do usuário para o cadastro de acesso ABAE")]
        [MaxLength(50, ErrorMessage = "A cidade do usuário não pode conter mais que 50 caracteres")]
        public string Cidade { get; set; }
        
        [Required(ErrorMessage = "É necessário informar o estado do usuário para o cadastro de acesso ABAE")]
        [MaxLength(5, ErrorMessage = "O estado do usuário não pode conter mais que 5 caracteres")]
        public string Estado { get; set; }
        
        public bool Excluido { get; set; }
        public string UeCodigo { get; set; }
        public string DreCodigo { get; set; }
    }
}

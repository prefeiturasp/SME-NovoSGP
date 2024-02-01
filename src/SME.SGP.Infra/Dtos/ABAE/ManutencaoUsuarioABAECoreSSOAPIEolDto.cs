using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class ManutencaoUsuarioABAECoreSSOAPIEolDto
    {
        public string Nome { get; set; }
        public string CodigoUe { get; set; }
        public string Cpf { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public bool Situacao { get; set; }
        public bool Excluido { get; set; }
    }

    public static class CadastroAcessoABAEExtension
    {
        static public ManutencaoUsuarioABAECoreSSOAPIEolDto toManutencaoUsuarioABAECoreSSOAPIEolDto(this CadastroAcessoABAE cadastroAcessoABAE, string codigoUe)
        {
            return new ManutencaoUsuarioABAECoreSSOAPIEolDto()
            {
                CodigoUe = codigoUe,
                Cpf = cadastroAcessoABAE.Cpf,
                Email = cadastroAcessoABAE.Email,
                Telefone = cadastroAcessoABAE.Telefone,
                Excluido = cadastroAcessoABAE.Excluido,
                Nome = cadastroAcessoABAE.Nome,
                Situacao = cadastroAcessoABAE.Situacao
            };
        }
    }
  }

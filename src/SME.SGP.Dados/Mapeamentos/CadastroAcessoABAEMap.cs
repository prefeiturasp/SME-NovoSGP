using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class CadastroAcessoABAEMap : BaseMap<CadastroAcessoABAE>
    {
        public CadastroAcessoABAEMap()
        {
            ToTable("cadastro_acesso_abae");
            Map(c => c.Nome).ToColumn("nome");
            Map(c => c.UeId).ToColumn("ue_id");
            Map(c => c.Cpf).ToColumn("cpf");
            Map(c => c.Email).ToColumn("email");
            Map(c => c.Telefone).ToColumn("telefone");
            Map(c => c.Situacao).ToColumn("situacao");
            Map(c => c.Cep).ToColumn("cep");
            Map(c => c.Endereco).ToColumn("endereco");
            Map(c => c.Excluido).ToColumn("excluido");
            Map(c => c.Numero).ToColumn("numero");
            Map(c => c.Complemento).ToColumn("complemento");
            Map(c => c.Bairro).ToColumn("bairro");
            Map(c => c.Cidade).ToColumn("cidade");
            Map(c => c.Estado).ToColumn("estado");
        }
    }
}
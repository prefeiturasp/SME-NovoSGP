using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class CadastroAcessoABAEMap : BaseMap<CadastroAcessoABAE>
    {
        public CadastroAcessoABAEMap()
        {
            ToTable("cadastro_acesso_abae");
            Map(nameof(CadastroAcessoABAE.Nome), "nome");
            Map(nameof(CadastroAcessoABAE.UeId), "ue_id");
            Map(nameof(CadastroAcessoABAE.Cpf), "cpf");
            Map(nameof(CadastroAcessoABAE.Email), "email");
            Map(nameof(CadastroAcessoABAE.Telefone), "telefone");
            Map(nameof(CadastroAcessoABAE.Situacao), "situacao");
            Map(nameof(CadastroAcessoABAE.Cep), "cep");
            Map(nameof(CadastroAcessoABAE.Endereco), "endereco");
            Map(nameof(CadastroAcessoABAE.Excluido), "excluido");
            Map(nameof(CadastroAcessoABAE.Numero), "numero");
            Map(nameof(CadastroAcessoABAE.Complemento), "complemento");
            Map(nameof(CadastroAcessoABAE.Bairro), "bairro");
            Map(nameof(CadastroAcessoABAE.Cidade), "cidade");
            Map(nameof(CadastroAcessoABAE.Estado), "estado");
        }
    }
}
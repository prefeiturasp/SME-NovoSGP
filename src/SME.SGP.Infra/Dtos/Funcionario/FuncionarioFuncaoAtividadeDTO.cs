using SME.SGP.Dominio;

namespace SME.SGP.Infra.Dtos
{
    public class FuncionarioFuncaoAtividadeDTO
    {
        public FuncionarioFuncaoAtividadeDTO() { }
        public FuncionarioFuncaoAtividadeDTO(string funcionarioRf, FuncaoAtividade funcaoAtividade)
        {
            FuncionarioRf = funcionarioRf;
            CodigoFuncaoAtividade = funcaoAtividade;
        }

        public string FuncionarioRf { get; set; }
        public FuncaoAtividade CodigoFuncaoAtividade { get; set; }
    }
}

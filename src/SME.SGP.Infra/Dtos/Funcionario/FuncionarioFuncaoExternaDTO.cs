using SME.SGP.Dominio;

namespace SME.SGP.Infra.Dtos
{
    public class FuncionarioFuncaoExternaDTO
    {
        public FuncionarioFuncaoExternaDTO() { }
        public FuncionarioFuncaoExternaDTO(string funcionarioCpf, FuncaoExterna funcaoExternaId)
        {
            FuncionarioCpf = funcionarioCpf;
            FuncaoExternaId = funcaoExternaId;
        }

        public string FuncionarioCpf { get; set; }        
        public FuncaoExterna FuncaoExternaId { get; set; }
    }
}

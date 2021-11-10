using SME.SGP.Dominio;

namespace SME.SGP.Infra.Dtos
{
    public class FuncionarioCargoDTO
    {
        public FuncionarioCargoDTO() { }
        public FuncionarioCargoDTO(string funcionarioRF, Cargo cargoId)
        {
            FuncionarioRF = funcionarioRF;
            CargoId = cargoId;
        }

        public string FuncionarioRF { get; set; }        
        public Cargo CargoId { get; set; }
    }
}

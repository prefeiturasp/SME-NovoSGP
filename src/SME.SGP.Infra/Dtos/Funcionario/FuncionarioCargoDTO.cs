using SME.SGP.Dominio;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

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

    public static class FuncionarioCargoExtension
    {
        public static bool ExistemNFuncionariosMesmoPerfil(this KeyValuePair<string, IEnumerable<FuncionarioCargoDTO>> valorDicionarioFuncionarioCargo)
        => valorDicionarioFuncionarioCargo.Value.Count() > 1;
    }
    
}

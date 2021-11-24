using SME.SGP.Infra.Dtos;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class FiltroPendenciaPerfilUsuarioCefaiAdmUeDto
    {
        public FiltroPendenciaPerfilUsuarioCefaiAdmUeDto(FuncionarioCargoDTO funcionarioAtual, bool eraCefai, bool eraAdmUe, PendenciaPerfilUsuarioDto pendenciaFuncionario)
        {
            FuncionarioAtual = funcionarioAtual;
            EraAdmUe = eraAdmUe;
            EraCefai = eraCefai;
            PendenciaFuncionario = pendenciaFuncionario;            
        }

        public FuncionarioCargoDTO FuncionarioAtual { get; set; }
        public bool EraAdmUe { get; set; }
        public bool EraCefai { get; set; }
        public PendenciaPerfilUsuarioDto PendenciaFuncionario { get; set; }
    }
}

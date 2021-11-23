using SME.SGP.Infra.Dtos;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class FiltroPendenciaPerfilUsuarioCefaiAdmUeDto
    {
        public FiltroPendenciaPerfilUsuarioCefaiAdmUeDto(FuncionarioCargoDTO funcionarioAtual, bool ehCefai, bool ehAdmUe, PendenciaPerfilUsuarioDto pendenciaFuncionario)
        {
            FuncionarioAtual = funcionarioAtual;
            EhAdmUe = ehAdmUe;
            EhCefai = ehCefai;
            PendenciaFuncionario = pendenciaFuncionario;            
        }

        public FuncionarioCargoDTO FuncionarioAtual { get; set; }
        public bool EhAdmUe { get; set; }
        public bool EhCefai { get; set; }
        public PendenciaPerfilUsuarioDto PendenciaFuncionario { get; set; }
    }
}

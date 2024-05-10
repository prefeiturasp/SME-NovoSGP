using SME.SGP.Dominio.Constantes.MensagensNegocio;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Dominio
{
    public class SuporteUsuario
    {
        public long Id { get; set; }
        public string UsuarioAdministrador { get; set; }
        public string UsuarioSimulado { get; set; }
        public DateTime DataAcesso { get; set; }
        public string TokenAcesso { get; set; }
        public Usuario Administrador { get; set; }

        public bool UsuarioPodeReceberSuporte(Usuario usuarioSuporte)
        {
            if (AdministradorPossuiApenasAdmDRE())
            {
                var perfisNaoPermitido = PerfisNaoPermitidosParaSuporteAdmDre();

                if (usuarioSuporte.Perfis.NaoEhNulo() &&
                    usuarioSuporte.Perfis.ToList().Exists(perfil => perfisNaoPermitido.Contains(perfil.CodigoPerfil)))
                {
                    throw new NegocioException(MensagemNegocioComuns.ACESSO_SUPORTE_INDISPONIVEL);
                }
            }

            return true;
        }

        private bool AdministradorPossuiApenasAdmDRE()
        {
            return Administrador.Perfis.ToList().Exists(perfil => perfil.CodigoPerfil == Perfis.PERFIL_ADMDRE) && 
                !Administrador.Perfis.ToList().Exists(perfil => perfil.CodigoPerfil == Perfis.PERFIL_ADMSME || perfil.CodigoPerfil == Perfis.PERFIL_ADMCOTIC);
        }

        private List<Guid> PerfisNaoPermitidosParaSuporteAdmDre()
        {
            return new List<Guid>
            {
                Perfis.PERFIL_NAAPA_DRE,
                Perfis.PERFIL_COORDENADOR_NAAPA,
                Perfis.PERFIL_PSICOLOGO_ESCOLAR,
                Perfis.PERFIL_PSICOPEDAGOGO,
                Perfis.PERFIL_ASSISTENTE_SOCIAL
            };
        }
    }
}

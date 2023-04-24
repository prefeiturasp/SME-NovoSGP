using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SME.SGP.Dominio
{
    public enum TipoResponsavelAtribuicao
    {
        [Display(Name = "Supervisor Escolar")]
        SupervisorEscolar = 1,

        [Display(Name = "PAAI")]
        PAAI = 2,

        [Display(Name = "Psicólogo Escolar")]
        PsicologoEscolar = 3,

        [Display(Name = "Psicopedagogo")]
        Psicopedagogo = 4,

        [Display(Name = "Assistente Social")]
        AssistenteSocial = 5,
    }

    public static class TipoResponsavelAtribuicaoExtension
    {
        public static Guid ToPerfil(this TipoResponsavelAtribuicao tipo)
        {
            switch (tipo)
            {
                case TipoResponsavelAtribuicao.Psicopedagogo:
                        return Perfis.PERFIL_PSICOPEDAGOGO;
                case TipoResponsavelAtribuicao.PsicologoEscolar:
                        return Perfis.PERFIL_PSICOLOGO_ESCOLAR;
                case TipoResponsavelAtribuicao.AssistenteSocial:
                        return Perfis.PERFIL_ASSISTENTE_SOCIAL;
                case TipoResponsavelAtribuicao.SupervisorEscolar:
                    return Perfis.PERFIL_SUPERVISOR;
                case TipoResponsavelAtribuicao.PAAI:
                    return Perfis.PERFIL_PAAI;
                default: throw new NotImplementedException();
            }
        }

        public static int[] ToIntegerArray(this TipoResponsavelAtribuicao[] tipos)
        {
            return tipos.Select(tipo => (int)tipo).ToArray();
        }
    }

}

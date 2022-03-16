using SME.SGP.Dominio;

namespace SME.SGP.Infra.Enumerados
{
    public static class EnumHelper
    {
        public static Cargo ObterCargoPorPerfil(int perfilCodigo)
        {
            switch (perfilCodigo)
            {
                case (int)PerfilUsuario.CP:
                    return Cargo.CP;
                case (int)PerfilUsuario.AD:
                    return Cargo.AD;
                case (int)PerfilUsuario.DIRETOR:
                    return Cargo.Diretor;
                default:
                    throw new NegocioException("Perfil não relacionado com Cargo");
            }
        }

        public static PerfilUsuario ObterPerfilPorCargo(Cargo cargoId)
        {
            switch (cargoId)
            {
                case Cargo.CP:
                    return PerfilUsuario.CP;
                case Cargo.AD:
                    return PerfilUsuario.AD;
                case Cargo.Diretor:
                    return PerfilUsuario.DIRETOR;
                case Cargo.Supervisor:
                    return PerfilUsuario.SUPERVISOR;
                case Cargo.SupervisorTecnico:
                    return PerfilUsuario.SUPERVISOR_TECNICO;
                default:
                    throw new NegocioException("Cargo não relacionado a um Perfil");
            }
        }
    }
}

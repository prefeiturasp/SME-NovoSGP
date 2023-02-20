namespace SME.SGP.Infra
{
    public class RotasRabbitSgpPendencias
    {
        public const string PendenciasGeraisCalendario = "sgp.pendencias.gerais.calendario";
        public const string PendenciasGeraisEventos = "sgp.pendencias.gerais.evento";
        public const string RotaRemoverAtribuicaoPendenciaUsuarios = "sgp.pendencias.perfil.usuario.remover.atribuicao";
        public const string RotaRemoverAtribuicaoPendenciaUsuariosUe = "sgp.pendencias.perfil.usuario.remover.atribuicao.ue";
        public const string RotaRemoverAtribuicaoPendenciaUsuariosUeFuncionario = "sgp.pendencias.perfil.usuario.remover.atribuicao.ue.funcionario";
        public const string PendenciasGerais = "sgp.pendencias.gerais";
        public const string RotaExecutaVerificacaoPendenciasGerais = "sgp.pendencias.gerais.executa.verificacao";
        public const string RotaExecutaVerificacaoPendenciasProfessor = "sgp.pendencias.professor.executa.verificacao";
        public const string RotaTratarAtribuicaoPendenciaUsuarios = "sgp.pendencias.atribuicao.tratar";
        public const string RotaCargaAtribuicaoPendenciaPerfilUsuario = "sgp.pendencia.perfil.usuario.atribuicao.carga";
        public const string RotaExecutarExclusaoPendenciasDevolutiva = "sgp.pendencias.devolutiva.excluir";
        public const string RotaExecutarExclusaoPendenciasNoFinalDoAnoLetivoPorAno = "sgp.pendencias.excluir.final.anoletivo.ano";
        public const string RotaExecutarExclusaoPendenciasNoFinalDoAnoLetivoPorUe = "sgp.pendencias.excluir.final.anoletivo.ue";
        public const string RotaExecutarExclusaoPendenciasDiarioDeClasseNoFinalDoAnoLetivo = "sgp.pendencias.excluir.final.anoletivo.diario.classe";
        public const string RotaExecutarExclusaoPendenciasNoFinalDoAnoLetivo = "sgp.pendencias.excluir.final.anoletivo";
    }
}

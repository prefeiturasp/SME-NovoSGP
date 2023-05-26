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
        public const string RotaExecutarReplicarParametrosAnoAnterior = "sgp.pendencias.replicar.ano.anterior";
        public const string RotaExcluirPendenciaCalendarioAnoAnteriorCalendario = "sgp.pendencias.calendario.excluir.ano.anterior";
        public const string RotaExcluirPendenciaCalendarioAnoAnteriorCalendarioUe = "sgp.pendencias.calendario.excluir.ano.anterior.ue";
        public const string RotaExcluirPendenciaCalendarioAnoAnteriorCalendarioIdsPendencias = "sgp.pendencias.calendario.excluir.ano.anterior.idspendencias";
        public const string RotaExecutarExclusaoPendenciasNoFinalDoAnoLetivoPorAno = "sgp.pendencias.excluir.final.anoletivo.ano";
        public const string RotaExecutarExclusaoPendenciasNoFinalDoAnoLetivoPorUe = "sgp.pendencias.excluir.final.anoletivo.ue";
        public const string RotaExecutarExclusaoPendenciasDiarioDeClasseNoFinalDoAnoLetivo = "sgp.pendencias.excluir.final.anoletivo.diario.classe";
        public const string RotaExecutarExclusaoPendenciasNoFinalDoAnoLetivo = "sgp.pendencias.excluir.final.anoletivo";
        public const string ExecutarAtualizacaoDosTotalizadoresDasPendencias = "sgp.atualizar.totalizadores.pendencias";
        public const string RotaBuscarAdicionarQuantidadeAulaDiaPendenciaUe = "sgp.atualizar.totalizadores.pendencia.ue";
        public const string RotaCargaAdicionarQuantidadeAulaDiaPendencia = "sgp.atualizar.totalizadores.pendencia.carga";
    }
}

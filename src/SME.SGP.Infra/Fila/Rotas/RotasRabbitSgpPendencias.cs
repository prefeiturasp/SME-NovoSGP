namespace SME.SGP.Infra
{
    public class RotasRabbitSgpPendencias
    {
        public const string RotaInserirPendenciaAula = "sgp.aulas.pendencias.inserir";

        public const string RotaExecutaPendenciasAula = "sgp.pendencias.aulas.executa";
        public const string RotaExecutaPendenciasAulaDre = "sgp.pendencias.aulas.dre.executa";
        public const string RotaExecutaPendenciasAulaDreUe = "sgp.pendencias.aulas.dre.ue.executa";
        public const string RotaExecutaPendenciasAulaDiarioBordo = "sgp.pendencias.aulas.diario.bordo.executa";
        public const string RotaExecutaPendenciasAulaDiarioBordoTurma = "sgp.pendencias.aulas.diario.bordo.turma.executa";
        public const string RotaExecutaPendenciasAulaDiarioBordoTurmaAulaComponente = "sgp.pendencias.aulas.diario.bordo.turma.aula.componente.executa";
        public const string RotaExecutaPendenciasAulaAvaliacao = "sgp.pendencias.aulas.avaliacao.executa";
        public const string RotaExecutaPendenciasAulaFrequencia = "sgp.pendencias.aulas.frequencia.executa";
        public const string RotaExecutaPendenciasAulaPlanoAula = "sgp.pendencias.aulas.plano.aula.executa";
        public const string RotaExecutaVerificacaoPendenciasGerais = "sgp.pendencias.gerais.executa.verificacao";
        public const string RotaExecutaExclusaoPendenciasAula = "sgp.pendencias.gerais.pendencias.aula.excluir";
        public const string RotaExecutaExclusaoPendenciaDiarioBordoAula = "sgp.pendencias.diario.bordo.aula.excluir";
        public const string RotaExecutaExclusaoPendenciasDiasLetivosInsuficientes = "sgp.pendencias.gerais.pendencias.calendario.excluir";
        public const string RotaExecutaExclusaoPendenciaParametroEvento = "sgp.pendencias.gerais.pendencias.evento.excluir";
        public const string RotaExecutaVerificacaoPendenciasProfessor = "sgp.pendencias.professor.executa.verificacao";
        public const string RotaExecutaVerificacaoPendenciasAusenciaFechamento = "sgp.pendencias.bimestre.ausencia.fechamento.verificacao";
        public const string RotaExecutaExclusaoPendenciasAusenciaAvaliacao = "sgp.pendencias.professor.avaliacao.excluir";
        public const string RotaExecutaExclusaoPendenciasAusenciaFechamento = "sgp.pendencias.bimestre.ausencia.fechamento.excluir";
        public const string RotaTratarAtribuicaoPendenciaUsuarios = "sgp.pendencias.atribuicao.tratar";
        public const string RotaCargaAtribuicaoPendenciaPerfilUsuario = "sgp.pendencia.perfil.usuario.atribuicao.carga";
        public const string RotaRemoverAtribuicaoPendenciaUsuarios = "sgp.pendencias.perfil.usuario.remover.atribuicao";
        public const string RotaRemoverAtribuicaoPendenciaUsuariosUe = "sgp.pendencias.perfil.usuario.remover.atribuicao.ue";
        public const string RotaRemoverAtribuicaoPendenciaUsuariosUeFuncionario = "sgp.pendencias.perfil.usuario.remover.atribuicao.ue.funcionario";

        public const string RotaGeracaoPendenciasFechamento = "sgp.fechamento.pendencias.gerar";

        public const string VerificarPendenciasFechamentoTurmaDisciplina = "sgp.verificar.pendencias.fechamento.turma.disciplina";

        public const string PendenciasGerais = "sgp.pendencias.gerais";
        public const string PendenciasGeraisAulas = "sgp.pendencias.gerais.aula";
        public const string PendenciasGeraisCalendario = "sgp.pendencias.gerais.calendario";
        public const string PendenciasGeraisEventos = "sgp.pendencias.gerais.evento";
    }
}

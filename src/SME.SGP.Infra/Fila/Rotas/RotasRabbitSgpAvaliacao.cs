namespace SME.SGP.Infra
{
    public static class RotasRabbitSgpAvaliacao
    {
        public const string RotaExecutaExclusaoPendenciasAusenciaAvaliacao = "sgp.pendencias.professor.avaliacao.excluir";
        public const string RotaAtividadesSync = "sgp.atividade.avaliativa.sync";
        public const string RotaAtividadesNotasSync = "sgp.atividade.avaliativa.notas.sync";
        public const string RotaValidarMediaAlunos = "sgp.validar.media.alunos";
        public const string RotaValidarMediaAlunosAtividadeAvaliativa = "sgp.validar.media.alunos.atividade.avaliativa";
        public const string RotaNotificarUsuarioAlteracaoExtemporanea = "sgp.notificar.usuario.alteracao.extemporanea";        

    }
}

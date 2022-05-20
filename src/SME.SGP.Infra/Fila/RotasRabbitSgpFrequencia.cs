namespace SME.SGP.Infra
{
    public class RotasRabbitSgpFrequencia
    {
        public const string RotaValidacaoAusenciaConciliacaoFrequenciaTurma = "sgp.frequencia.turma.conciliacao.validar";
        public const string RotaCalculoFrequenciaPorTurmaComponente = "sgp.frequencia.turma.componente";
        public const string ConsolidarFrequenciasPorTurma = "sgp.frequencia.turma.consolidar";
        public const string RotaConsolidacaoDashBoardFrequencia = "sgp.consolidacao.frequencia.dashboard";
        public const string ConsolidarFrequenciasTurmasPorUe = "sgp.frequencia.turma.ue.consolidar";
        public const string ConsolidacaoFrequenciasTurmasCarregar = "sgp.frequencia.turma.carregar";
        public const string ConsolidarFrequenciasTurmasNoAno = "sgp.frequencia.turma.ano.consolidar";
        public const string ConsolidarFrequenciasTurmasPorDre = "sgp.frequencia.turma.dre.consolidar";

        /*
        public const string RotaConciliacaoFrequenciaAnoSync = "sgp.frequencia.turma.conciliacao.ano.sync";
        public const string RotaConciliacaoFrequenciaTurmaDreSync = "sgp.frequencia.turma.conciliacao.dre.sync";
        public const string RotaConciliacaoFrequenciaTurmaUeSync = "sgp.frequencia.turma.conciliacao.ue.sync";
        public const string RotaConciliacaoFrequenciaTurmasSync = "sgp.frequencia.turma.conciliacao.sync";
        */

        public const string RotaConciliacaoFrequenciaTurmaPorPeriodo = "sgp.frequencia.turma.conciliacao.periodo";
        public const string RotaConciliacaoCalculoFrequenciaPorTurmaComponente = "sgp.frequencia.turma.conciliacao.componente";

        public const string RotaConciliacaoFrequenciaTurmasAlunosSync = "sgp.frequencia.turma.alunos.conciliacao.sync";
        public const string RotaConciliacaoFrequenciaTurmasAlunosBuscar = "sgp.frequencia.turma.alunos.buscar.sync";
        public const string RotaCalcularFrequenciaGeralSync = "sgp.frequencia.calcular.geral.sync";

    }
}

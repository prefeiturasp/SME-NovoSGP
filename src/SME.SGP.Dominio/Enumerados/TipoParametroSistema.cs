namespace SME.SGP.Dominio
{
    public enum TipoParametroSistema
    {
        //Calendário

        EjaDiasLetivos = 1,
        FundamentalMedioDiasLetivos = 2,

        //Frequência

        PercentualFrequenciaAlerta = 3,
        PercentualFrequenciaCritico = 4,
        QuantidadeAulasNotificarProfessor = 5,
        QuantidadeAulasNotificarGestorUE = 6,
        QuantidadeAulasNotificarSupervisorUE = 7,
        QuantidadeDiasNotificarAlteracaoChamadaEfetivada = 8,

        //Aula Prevista
        QuantidadeDiasNotificarProfessor = 9,

        //Compensação Ausência
        CompensacaoAusenciaPercentualRegenciaClasse = 10,

        CompensacaoAusenciaPercentualFund2 = 11,
        QuantidadeMaximaCompensacaoAusencia = 12,

        //Recuperação Paralela PAP
        RecuperacaoParalelaFrequencia = 13,

        // Média
        MediaBimestre = 14,

        PercentualAlunosInsuficientes = 15,

        // Notificação alunos ausentes
        QuantidadeDiasNotificaoCPAlunosAusentes = 16,
        QuantidadeDiasNotificaoDiretorAlunosAusentes = 17,

        //Nota Fechamento
        QuantidadeDiasAlteracaoNotaFinal = 19,

        //Sistema
        HabilitarServicosEmBackground = 100

    }
}
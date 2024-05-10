namespace SME.SGP.Infra
{
    public static class RotasRabbitSerapEstudantes
    {
        public static string ExchangeSerapEstudantes => "serap.estudante.workers";      

        #region Provas
        public static string FilaProvaSync => "serap.estudante.prova.legado.sync";
        #endregion      
           
        #region Deadletter
        public static string FilaDeadletterSync => "serap.deadletter.sync";
        
        #endregion

    }
}
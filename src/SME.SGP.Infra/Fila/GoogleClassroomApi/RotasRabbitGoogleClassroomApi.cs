namespace SME.SGP.Infra
{
    public static class RotasRabbitGoogleClassroomApi
    {
        public static string ExchangeGoogleSync => "googleclass.exchange";
        public static string FilaGoogleSync => "googleclass.sync.geral";

        #region Professores

        public static string FilaProfessorCursoIncluir => "googleclass.professor.curso.incluir";

        #endregion Professores
    }
}
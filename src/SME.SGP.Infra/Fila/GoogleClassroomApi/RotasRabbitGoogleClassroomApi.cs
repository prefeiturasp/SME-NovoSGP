﻿namespace SME.SGP.Infra
{
    public static class RotasRabbitSgpGoogleClassroomApi
    {
        public static string ExchangeGoogleSync => "googleclass.exchange";
        public static string FilaGoogleSync => "googleclass.sync.geral";

        #region GSA
        public static string FilaGsaSync => "googleclass.gsa.sync";
        #endregion

        #region Professores

        public static string FilaProfessorCursoIncluir => "googleclass.professor.curso.incluir";
        public static string FilaProfessorCursoRemover => "googleclass.professor.curso.remover";

        #endregion Professores
    }
}
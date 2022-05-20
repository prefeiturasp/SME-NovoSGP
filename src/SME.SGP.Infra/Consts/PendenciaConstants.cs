﻿namespace SME.SGP.Infra
{
    public static class PendenciaConstants
    {
        public static string ObterDescricaoPendenciaDiarioBordo(string descricaoComponenteCurricular, string nomeComModalidade, string nomeEscola)
        {
            return $"O registro do Diário de Bordo do componente {descricaoComponenteCurricular} da turma {nomeComModalidade} da {nomeEscola} das aulas abaixo está pendente:";
        }

        public static string ObterDescricaoPendenciaAvaliacao(string descricaoComponenteCurricular, string nomeComModalidade, string nomeEscola)
        {
            return $"As avaliações abaixo do componente {descricaoComponenteCurricular} da turma {nomeComModalidade} da {nomeEscola} estão sem notas lançadas:";
        }

        public static string ObterDescricaoPendenciaPlanoAula(string descricaoComponenteCurricular, string nomeComModalidade, string nomeEscola)
        {
            return $"As aulas abaixo do componente {descricaoComponenteCurricular} da turma {nomeComModalidade} da {nomeEscola} estão sem plano de aula registrado:";
        }

        public static string ObterDescricaoPendenciaFrequencia(string descricaoComponenteCurricular, string nomeComModalidade, string nomeEscola)
        {
            return $"O registro de frequência do componente {descricaoComponenteCurricular} da turma {nomeComModalidade} da {nomeEscola} das aulas abaixo está pendente:";
        }
    }
}

using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Aplicacao
{
    public static class FrequenciaAlunoExtensao
    {
        public static Dictionary<string, FrequenciaAluno> ToDicionarioPorAluno(this IEnumerable<FrequenciaAluno> frequenciasAlunos)
        {
            if (frequenciasAlunos.EhNulo())
                return new Dictionary<string, FrequenciaAluno>();

            return frequenciasAlunos
                .Where(frequencia => !string.IsNullOrWhiteSpace(frequencia.CodigoAluno))
                .GroupBy(frequencia => frequencia.CodigoAluno)
                .ToDictionary(grupo => grupo.Key, grupo => grupo.OrderByDescending(frequencia => frequencia.Id).First());
        }

        public static FrequenciaAluno ObterFrequenciaAlunoOuNulo(this IDictionary<string, FrequenciaAluno> frequenciaPorAluno, string codigoAluno)
        {
            if (frequenciaPorAluno.EhNulo() || string.IsNullOrWhiteSpace(codigoAluno))
                return null;

            return frequenciaPorAluno.TryGetValue(codigoAluno, out var frequenciaAluno) ? frequenciaAluno : null;
        }
    }
}

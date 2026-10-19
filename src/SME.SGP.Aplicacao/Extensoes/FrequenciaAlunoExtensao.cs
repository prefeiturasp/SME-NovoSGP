using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public static class FrequenciaAlunoExtensao
    {
        /// <summary>
        /// Indexa as frequências pelo código do aluno para consulta em memória, no lugar de uma consulta por aluno.
        /// Quando houver mais de um registro para o mesmo aluno prevalece o de maior Id, que é o mais recente.
        /// </summary>
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

    public static class FrequenciaAlunoConsulta
    {
        /// <summary>
        /// Obtém, em uma única consulta, a frequência geral dos alunos informados indexada pelo código do aluno.
        /// Sem código de aluno válido a consulta não é executada, pois o validator da query exige a lista preenchida.
        /// </summary>
        public static async Task<Dictionary<string, FrequenciaAluno>> ObterFrequenciaGeralPorAlunos(IMediator mediator, IEnumerable<string> codigosAlunos, string turmaCodigo, string componenteCurricularCodigo)
        {
            var codigos = codigosAlunos?
                .Where(codigo => !string.IsNullOrWhiteSpace(codigo))
                .Distinct()
                .ToArray() ?? Array.Empty<string>();

            if (!codigos.Any())
                return new Dictionary<string, FrequenciaAluno>();

            var frequenciasAlunos = await mediator.Send(new ObterFrequenciaGeralPorAlunosTurmaEComponenteQuery(codigos, turmaCodigo, componenteCurricularCodigo));

            return frequenciasAlunos.ToDicionarioPorAluno();
        }
    }
}

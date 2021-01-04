using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados
{
    public class RepositorioOcorrencia : RepositorioBase<Ocorrencia>, IRepositorioOcorrencia
    {
        public RepositorioOcorrencia(ISgpContext conexao) : base(conexao) { }

		public async Task<IEnumerable<Ocorrencia>> Listar(string titulo, string alunoNome, DateTime? dataOcorrenciaInicio, DateTime? dataOcorrenciaFim)
		{
			StringBuilder query = new StringBuilder();
			query.AppendLine(@"select
							o.id,
							o.titulo,
							o.data_ocorrencia,
							o.hora_ocorrencia,
							o.descricao,
							o.ocorrencia_tipo_id,
							o.excluido,
							o.criado_rf,
							o.criado_em,
							o.alterado_em,
							o.alterado_por,
							o.alterado_rf,
							ot.id,
							ot.descricao,
							oa.id,
							oa.codigo_aluno
						from
							ocorrencia o
						inner join ocorrencia_tipo ot on ot.id = o.ocorrencia_tipo_id 
						inner join ocorrencia_aluno oa on oa.ocorrencia_id = o.id
						where not excluido ");

			if (!string.IsNullOrEmpty(titulo))
				query.AppendLine("and lower(f_unaccent(o.titulo)) LIKE lower(f_unaccent(@titulo))");

			if (dataOcorrenciaInicio.HasValue)
				query.AppendLine("and data_ocorrencia::date >= @dataOcorrenciaInicio  ");

			if (dataOcorrenciaFim.HasValue)
				query.AppendLine("and data_ocorrencia::date <= @dataOcorrenciaFim");


			var lstOcorrencias = new Dictionary<long, Ocorrencia>();

			await database.Conexao.QueryAsync<Ocorrencia, OcorrenciaTipo, OcorrenciaAluno, Ocorrencia>(query.ToString(), (ocorrencia, tipo, aluno) =>
			{
                if (!lstOcorrencias.TryGetValue(ocorrencia.Id, out Ocorrencia ocorrenciaEntrada))
                {
                    ocorrenciaEntrada = ocorrencia;
					ocorrenciaEntrada.OcorrenciaTipo = tipo;
					lstOcorrencias.Add(ocorrenciaEntrada.Id, ocorrenciaEntrada);
                }

				ocorrenciaEntrada.Alunos.Add(aluno);
				return ocorrenciaEntrada;
			}, new { titulo, alunoNome, dataOcorrenciaInicio, dataOcorrenciaFim }, splitOn: "id, id");

			return lstOcorrencias.Values.ToList();
		}
	}
}

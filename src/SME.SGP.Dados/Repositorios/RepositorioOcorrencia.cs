using Dapper;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados
{
    public class RepositorioOcorrencia : RepositorioBase<Ocorrencia>, IRepositorioOcorrencia
    {
        private const long TODAS_UES = -99;
        public RepositorioOcorrencia(ISgpContext conexao, IServicoAuditoria servicoAuditoria) : base(conexao, servicoAuditoria) { }

        public async Task<PaginacaoResultadoDto<Ocorrencia>> ListarPaginado(FiltroOcorrenciaListagemDto filtro, Paginacao paginacao, long[] idUes = null)
        {
            var tabelas = @" ocorrencia o
						inner join ocorrencia_tipo ot on ot.id = o.ocorrencia_tipo_id 
                        left  join turma tu on tu.id  = o.turma_id
						left join ocorrencia_aluno oa on oa.ocorrencia_id = o.id 
                        left join ocorrencia_servidor os on os.ocorrencia_id = o.id";

            var gerador = new GeradorDeCondicoes(" where not o.excluido and extract(year from o.data_ocorrencia) = @anoLetivo ");

            var filtrarPorUe = (filtro.UeId == TODAS_UES); 
            
            gerador.AdicioneCondicao(!filtrarPorUe,"and o.ue_id = @ueId ");
            gerador.AdicioneCondicao(filtrarPorUe, " and o.ue_id = any(@ueIds) ");
            gerador.AdicioneCondicao(filtro.TurmaId.HasValue, "and tu.id = @turmaId ");
            gerador.AdicioneCondicao(filtro.Modalidade.HasValue, "and tu.modalidade_codigo = @modalidade ");
            gerador.AdicioneCondicao(filtro.Semestre.HasValue, "and tu.semestre = @semestre ");
            gerador.AdicioneCondicao(filtro.TipoOcorrencia.HasValue, "and ot.id = @tipoOcorrencia ");
            gerador.AdicioneCondicao(!string.IsNullOrEmpty(filtro.Titulo), "and lower(f_unaccent(o.titulo)) LIKE ('%' || lower(f_unaccent(@titulo)) || '%')");
            gerador.AdicioneCondicao(filtro.DataOcorrenciaInicio.HasValue, "and data_ocorrencia::date >= @dataOcorrenciaInicio  ");
            gerador.AdicioneCondicao(filtro.DataOcorrenciaFim.HasValue, "and data_ocorrencia::date <= @dataOcorrenciaFim");

            var condicao = gerador.ObterCondicao();
            var orderBy = "order by o.data_ocorrencia desc";

            if (paginacao.EhNulo() || (paginacao.QuantidadeRegistros == 0 && paginacao.QuantidadeRegistrosIgnorados == 0))
                paginacao = new Paginacao(1, 10);

            var query = $@" drop table if exists tempOcorrenciasSelecionadas;

                        select
                            distinct o.id, o.data_ocorrencia, o.turma_id  as turmaId
                        into temp tempOcorrenciasSelecionadas
                        from {tabelas}
                        {condicao} {orderBy};

                        select
							o.id,
							o.turma_id,
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
							o.ue_id ,
							ot.id,
							ot.descricao,
							oa.id,
							oa.codigo_aluno,
                            os.*,
                            tu.*
                        from tempOcorrenciasSelecionadas tos
                        inner join ocorrencia o on tos.id = o.id
						inner join ocorrencia_tipo ot on ot.id = o.ocorrencia_tipo_id 
                        left join turma tu on tu.id = tos.turmaId
						left join ocorrencia_aluno oa on oa.ocorrencia_id = o.id
                        left join ocorrencia_servidor os on os.ocorrencia_id = o.id;";

            var lstOcorrencias = new Dictionary<long, Ocorrencia>();

            await database.Conexao.QueryAsync<Ocorrencia, OcorrenciaTipo, OcorrenciaAluno, OcorrenciaServidor, Turma, Ocorrencia>(query.ToString(), (ocorrencia, tipo, aluno, servidor, turma) =>
            {
                if (!lstOcorrencias.TryGetValue(ocorrencia.Id, out Ocorrencia ocorrenciaEntrada))
                {
                    ocorrenciaEntrada = ocorrencia;
                    ocorrenciaEntrada.OcorrenciaTipo = tipo;
                    ocorrenciaEntrada.Turma = turma;
                    lstOcorrencias.Add(ocorrenciaEntrada.Id, ocorrenciaEntrada);
                }

                if (aluno.NaoEhNulo() &&
                    !ocorrenciaEntrada.Alunos.ToList().Exists(item => item.CodigoAluno == aluno.CodigoAluno))
                    ocorrenciaEntrada.Alunos.Add(aluno);

                if (servidor.NaoEhNulo() &&
                   !ocorrenciaEntrada.Servidores.ToList().Exists(item => item.CodigoServidor == servidor.CodigoServidor))
                    ocorrenciaEntrada.Servidores.Add(servidor);

                return ocorrenciaEntrada;
            }, new
            {
                titulo = filtro.Titulo,
                dataOcorrenciaInicio = filtro.DataOcorrenciaInicio.GetValueOrDefault(),
                dataOcorrenciaFim = filtro.DataOcorrenciaFim.GetValueOrDefault(),
                turmaId = filtro.TurmaId.GetValueOrDefault(),
                ueId = filtro.UeId,
                ueIds = idUes,
                modalidade = filtro.Modalidade.GetValueOrDefault(),
                semestre = filtro.Semestre.GetValueOrDefault(),
                tipoOcorrencia = filtro.TipoOcorrencia.GetValueOrDefault(),
                anoLetivo = filtro.AnoLetivo,
                qtdeRegistrosIgnorados = paginacao.QuantidadeRegistrosIgnorados,
                qtdeRegistros = paginacao.QuantidadeRegistros
            }, splitOn: "id, id");

            return new PaginacaoResultadoDto<Ocorrencia>()
            {
                Items = lstOcorrencias.Values.ToList().OrderByDescending(x=>x.DataOcorrencia),
                TotalRegistros = lstOcorrencias.Values.Count,
                TotalPaginas = (int)Math.Ceiling((double)lstOcorrencias.Values.Count / paginacao.QuantidadeRegistros)
            };
        }

        public override async Task<Ocorrencia> ObterPorIdAsync(long id)
        {
            const string sql = @"select
									o.id,
									o.alterado_em as AlteradoEm,
									o.alterado_por as AlteradoPor,
									o.alterado_rf as AlteradoRf,
									o.criado_em as CriadoEm,
									o.criado_por as CriadoPor,
									o.criado_rf as CriadoRf,
									o.data_ocorrencia as DataOcorrencia,
									o.hora_ocorrencia as HoraOcorrencia,
									o.ocorrencia_tipo_id as OcorrenciaTipoId,
									o.turma_id as TurmaId,
									o.titulo as Titulo,
									o.ue_id  as UeId,
									o.descricao as Descricao,
									o.excluido as Excluido,
									oa.id,
									oa.codigo_aluno as CodigoAluno,
									oa.ocorrencia_id as OcorrenciaId,
									os.id,
									os.rf_codigo  as CodigoServidor,
									os.ocorrencia_id  as OcorrenciaId,
									u.*,t.*
								FROM public.ocorrencia o
								left JOIN public.ocorrencia_aluno oa ON o.id = oa.ocorrencia_id
								left join public.ocorrencia_servidor os on o.id = os.ocorrencia_id
								inner join public.ue u on o.ue_id = u.id
								left join public.turma t on o.turma_id = t.id
								WHERE o.id = @id
									AND not o.excluido;";

            Ocorrencia resultado = null;
            await database.Conexao.QueryAsync<Ocorrencia, OcorrenciaAluno,OcorrenciaServidor,Ue,Turma,Ocorrencia>(sql,
                (ocorrencia, ocorrenciaAluno,ocorrenciaServidor,ue,turma) =>
                {
                    if (resultado is null)
                    {
                        resultado = ocorrencia;
                    }

                    if (turma.NaoEhNulo()) resultado.Turma = turma;
                    if (ue.NaoEhNulo()) resultado.Ue = ue;
                    
                    resultado.Alunos = resultado?.Alunos ?? new List<OcorrenciaAluno>();
                    if(ocorrenciaAluno.NaoEhNulo() && 
                        !resultado.Alunos.ToList().Exists(aluno => aluno.CodigoAluno == ocorrenciaAluno.CodigoAluno)) 
                        resultado.Alunos.Add(ocorrenciaAluno);
                    
                    resultado.Servidores = resultado?.Servidores ?? new List<OcorrenciaServidor>();
                    if(ocorrenciaServidor.NaoEhNulo() && 
                       !resultado.Servidores.ToList().Exists(servidor => servidor.CodigoServidor == ocorrenciaServidor.CodigoServidor)) 
                        resultado.Servidores.Add(ocorrenciaServidor);
                    
                    return resultado;
                },
                new { id });

            return resultado;
        }

        public async Task<PaginacaoResultadoDto<OcorrenciasPorAlunoDto>> ObterOcorrenciasPorTurmaAlunoEPeriodoPaginadas(long turmaId, long codigoAluno, DateTime periodoInicio, DateTime periodoFim, Paginacao paginacao)
        {
            var query = MontaQueryCompleta(paginacao, turmaId, codigoAluno, periodoInicio, periodoFim);
            var parametros = new { turmaId, codigoAluno, periodoInicio, periodoFim };
            var retorno = new PaginacaoResultadoDto<OcorrenciasPorAlunoDto>();

            using (var multi = await database.Conexao.QueryMultipleAsync(query, parametros))
            {
                retorno.Items = multi.Read<OcorrenciasPorAlunoDto>();
                retorno.TotalRegistros = multi.ReadFirst<int>();
            }
            retorno.TotalPaginas = (int)Math.Ceiling((double)retorno.TotalRegistros / paginacao.QuantidadeRegistros);
            return retorno;
        }

        private static string MontaQueryCompleta(Paginacao paginacao, long turmaId, long alunoCodigo, DateTime periodoIncio, DateTime periodoFim)
        {
            StringBuilder sql = new StringBuilder();

            MontaQueryConsulta(paginacao, sql, contador: false, turmaId, alunoCodigo, periodoIncio, periodoFim);

            sql.AppendLine(";");

            MontaQueryConsulta(paginacao, sql, contador: true, turmaId, alunoCodigo, periodoIncio, periodoFim);

            return sql.ToString();
        }

        private static void MontaQueryConsulta(Paginacao paginacao, StringBuilder sql, bool contador, long turmaId, long alunoCodigo, DateTime periodoIncio, DateTime periodoFim)
        {
            ObtenhaCabecalho(sql, contador);

            ObtenhaFiltro(sql, turmaId, alunoCodigo, periodoIncio, periodoFim);

            if (!contador)
                sql.AppendLine(" order by o.data_ocorrencia desc ");

            if (paginacao.QuantidadeRegistros > 0 && !contador)
                sql.AppendLine($" OFFSET {paginacao.QuantidadeRegistrosIgnorados} ROWS FETCH NEXT {paginacao.QuantidadeRegistros} ROWS ONLY ");
        }

        private static void ObtenhaCabecalho(StringBuilder sql, bool contador)
        {
            sql.AppendLine("select ");
            if (contador)
                sql.AppendLine(" count(o.id) ");
            else
            {
                sql.AppendLine(" o.data_ocorrencia dataOcorrencia ");
                sql.AppendLine(", CONCAT(o.criado_por, ' (', o.criado_rf, ')') registrado_por ");
                sql.AppendLine(", o.titulo ");
            }

            sql.AppendLine(" from ocorrencia o ");
            sql.AppendLine(" inner join ocorrencia_aluno oa on oa.ocorrencia_id = o.id ");
            sql.AppendLine(" inner join turma t on t.id = o.turma_id");
        }

        private static void ObtenhaFiltro(StringBuilder sql, long turmaId, long alunoCodigo, DateTime periodoIncio, DateTime periodoFim)
        {
            sql.AppendLine(" where ");
            sql.AppendLine(" o.turma_id = @turmaId and ");
            sql.AppendLine(" oa.codigo_aluno = @codigoAluno and ");
            sql.AppendLine(" o.data_ocorrencia::date between @periodoInicio and @periodoFim and");
            sql.AppendLine(" not o.excluido");
        }
    }
}

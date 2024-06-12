using Dapper;
using Elastic.Apm.Api;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.MapeamentoEstudantes;
using SME.SGP.Infra.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioMapeamentoEstudante : RepositorioBase<MapeamentoEstudante>, IRepositorioMapeamentoEstudante
    {
        public const int SECAO_ETAPA_1 = 1;
        public const int SECAO_ORDEM_1 = 1;
        public const int FILTRO_TODOS = -99;

        public const string QUESTAO_BUSCA_ATIVA_DATA_REGISTRO_NOME_COMPONENTE = "'DATA_REGISTRO_ACAO'";

        public RepositorioMapeamentoEstudante(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        { }

        public async Task<long?> ObterIdentificador(string codigoAluno, long turmaId, int bimestre)
        {
            var sql = @"SELECT id 
                        FROM mapeamento_estudante
                        WHERE aluno_codigo = @codigoAluno
                        AND turma_id = @turmaId
                        AND bimestre = @bimestre";

            return await database.Conexao.QueryFirstOrDefaultAsync<long?>(sql, new { codigoAluno, turmaId, bimestre });
        }

        public async Task<IEnumerable<long>> ObterIdentificadoresDosMapeamentosDoBimestreAtual(DateTime dataBase)
        {
            var sql = @"select me.id
                        from mapeamento_estudante me 
                        inner join turma t on t.id = me.turma_id 
                        where me.bimestre = (select pe.bimestre
                                  from periodo_escolar pe
                                  left join tipo_calendario tc on pe.tipo_calendario_id = tc.id 
                                  where tc.modalidade = @modalidade
                                  and tc.ano_letivo = @anoLetivo
                                  and pe.periodo_inicio::date <= @dataReferencia and pe.periodo_fim::date >= @dataReferencia
                                  and not tc.excluido)
                        and t.ano_letivo = @anoLetivo";

            var parametro = new
            {
                anoLetivo = dataBase.Year,
                dataReferencia = dataBase.Date,
                modalidade = (int)ModalidadeTipoCalendario.FundamentalMedio
            };

            return await database.Conexao.QueryAsync<long>(sql, parametro);
        }

        public async Task<MapeamentoEstudante> ObterMapeamentoEstudantePorId(long id)
        {
            const string query = @" select me.*, mes.*, qme.*, rme.*, sme.*, q.*, op.*
                            from mapeamento_estudante me
                            inner join mapeamento_estudante_secao mes on mes.mapeamento_estudante_id = me.id
                                and not mes.excluido
                            inner join secao_mapeamento_estudante sme on sme.id = mes.secao_mapeamento_estudante_id
                                and not sme.excluido
                            inner join mapeamento_estudante_questao qme on qme.mapeamento_estudante_secao_id = mes.id
                                and not qme.excluido
                            inner join questao q on q.id = qme.questao_id
                                and not q.excluido
                            inner join mapeamento_estudante_resposta rme on rme.questao_mapeamento_estudante_id = qme.id
                                and not rme.excluido
                            left join opcao_resposta op on op.id = rme.resposta_id
                                and not op.excluido
                            where me.id = @id
                            and not me.excluido;";

            var mapeamento = new MapeamentoEstudante();

            await database.Conexao
                .QueryAsync<MapeamentoEstudante, MapeamentoEstudanteSecao, QuestaoMapeamentoEstudante,
                    RespostaMapeamentoEstudante, SecaoMapeamentoEstudante, Questao, OpcaoResposta, MapeamentoEstudante>(
                    query, (mapeamentoEstudante, mapeamentoSecao, questaoMapeamentoEstudante, respostaMapeamentoEstudante,
                        secaoMapeamentoEstudante, questao, opcaoResposta) =>
                    {
                        if (mapeamento.Id == 0)
                            mapeamento = mapeamentoEstudante;

                        var secao = mapeamento.Secoes.FirstOrDefault(c => c.Id == mapeamentoSecao.Id);

                        if (secao.EhNulo())
                        {
                            mapeamentoSecao.SecaoMapeamentoEstudante = secaoMapeamentoEstudante;
                            secao = mapeamentoSecao;
                            mapeamento.Secoes.Add(secao);
                        }

                        var questaoRegistroAcao = secao.Questoes.FirstOrDefault(c => c.Id == questaoMapeamentoEstudante.Id);

                        if (questaoRegistroAcao.EhNulo())
                        {
                            questaoRegistroAcao = questaoMapeamentoEstudante;
                            questaoRegistroAcao.Questao = questao;
                            secao.Questoes.Add(questaoRegistroAcao);
                        }

                        var resposta = questaoRegistroAcao.Respostas.FirstOrDefault(c => c.Id == respostaMapeamentoEstudante.Id);

                        if (resposta.EhNulo())
                        {
                            resposta = respostaMapeamentoEstudante;
                            resposta.Resposta = opcaoResposta;
                            questaoRegistroAcao.Respostas.Add(resposta);
                        }

                        return mapeamento;
                    }, new { id });

            return mapeamento;
        }

        public async Task<IEnumerable<string>> ObterCodigoArquivoPorMapeamentoEstudanteId(long id)
        {
            var sql = @"select
                    	a.codigo
                    from
	                    mapeamento_estudante me
                    inner join mapeamento_estudante_secao mes on
	                    me.id = mes.mapeamento_estudante_id
                    inner join mapeamento_estudante_questao qme on
	                    mes.id = qme.mapeamento_estudante_secao_id
                    inner join mapeamento_estudante_resposta rme on
	                    qme.id = rme.questao_registro_acao_id
                    inner join arquivo a on
	                    rme.arquivo_id = a.id
                    where
	                    me.id = @id";

            return await database.Conexao.QueryAsync<string>(sql.ToString(), new { id });
        }

        public async Task<InformacoesAtualizadasMapeamentoEstudanteAlunoDto> ObterInformacoesAtualizadasAlunoMapeamentoEstudante(string codigoAluno, int anoLetivo, int bimestre)
        {
            var retorno = new InformacoesAtualizadasMapeamentoEstudanteAlunoDto();
            var query = @$"select t.modalidade_codigo as Modalidade, t.nome as NomeTurma,  
	                             ccp.id as IdParecerConclusivo, ccp.nome as DescricaoParecerConclusivo	
	                      from consolidado_conselho_classe_aluno_turma cccat 
	                      inner join turma t on t.id = cccat.turma_id 
	                      inner join conselho_classe_parecer ccp on ccp.id = coalesce(cccat.parecer_conclusivo_id, 0)
	                      where cccat.aluno_codigo = @codigoAluno
	                      and not cccat.excluido 
	                      and t.ano_letivo = @anoLetivo -1
	                      order by cccat.dt_atualizacao desc;
                          with vw_resposta_data as (
                             select rabas.registro_acao_busca_ativa_id, 
                                    rabar.texto DataRegistro
                             from registro_acao_busca_ativa_secao rabas    
                             join registro_acao_busca_ativa_questao rabaq on rabas.id = rabaq.registro_acao_busca_ativa_secao_id  
                             join questao q on rabaq.questao_id = q.id 
                             join registro_acao_busca_ativa_resposta rabar on rabar.questao_registro_acao_id = rabaq.id 
                             join secao_registro_acao_busca_ativa sraba on sraba.id = rabas.secao_registro_acao_id
                             where q.nome_componente = {QUESTAO_BUSCA_ATIVA_DATA_REGISTRO_NOME_COMPONENTE} 
                                   and sraba.etapa = 1
                                   and sraba.ordem = 1
                                   and not rabas.excluido 
                                   and not rabaq.excluido
                                   and not rabar.excluido
                                   and not sraba.excluido)
                         select  count(raba.id) 
                         from registro_acao_busca_ativa raba 
                         inner join vw_resposta_data qdata on qdata.registro_acao_busca_ativa_id = raba.id
                         inner join turma t on t.id = raba.turma_id 
                         inner join tipo_calendario tc on tc.modalidade = {(int)ModalidadeTipoCalendario.FundamentalMedio} and tc.ano_letivo = t.ano_letivo and not tc.excluido
                         inner join periodo_escolar pe on pe.tipo_calendario_id = tc.id
                         where raba.aluno_codigo = @codigoAluno 
                               and length(qdata.DataRegistro) > 0 
                               and not raba.excluido 
                               and t.ano_letivo = @anoLetivo
                               and pe.bimestre = @bimestre
                               and pe.periodo_inicio::date <= to_date(qdata.DataRegistro,'yyyy-mm-dd') 
                               and pe.periodo_fim::date >= to_date(qdata.DataRegistro,'yyyy-mm-dd');
                         select count(pa.id) from plano_aee pa where pa.aluno_codigo = @codigoAluno and not pa.excluido and not situacao in({(int)SituacaoPlanoAEE.Encerrado}, {(int)SituacaoPlanoAEE.EncerradoAutomaticamente});
                         select count(en.id) from encaminhamento_naapa en  where en.aluno_codigo = @codigoAluno and not en.excluido and not situacao in({(int)SituacaoNAAPA.Encerrado});
                         select cca.anotacoes_pedagogicas from conselho_classe_aluno cca 
                         inner join conselho_classe cc on cc.id = cca.conselho_classe_id 
                         inner join fechamento_turma ft on ft.id = cc.fechamento_turma_id 
                         inner join turma t on t.id = ft.turma_id 
                         left join periodo_escolar pe on pe.id = ft.periodo_escolar_id 
                         where not cca.excluido and not cc.excluido and not ft.excluido 
                         and cca.aluno_codigo = @codigoAluno
                         and t.ano_letivo = {(bimestre == 1 ? anoLetivo - 1 : anoLetivo)}
                         and pe.bimestre = {(bimestre == 1 ? 4 : bimestre - 1)}
                         order by cca.id desc;";
            using (var mapeamentoEstudante = await database.Conexao.QueryMultipleAsync(query, new { codigoAluno, anoLetivo, bimestre }))
            {
                var parecerConclusivoTurmaAnoAnterior = mapeamentoEstudante.ReadFirstOrDefault<ParecerConclusivoAlunoTurmaAnoAnteriorDto>();
                retorno.TurmaAnoAnterior = parecerConclusivoTurmaAnoAnterior?.Turma();
                retorno.IdParecerConclusivoAnoAnterior = string.IsNullOrEmpty(parecerConclusivoTurmaAnoAnterior?.DescricaoParecerConclusivo) ? null : parecerConclusivoTurmaAnoAnterior.IdParecerConclusivo;
                retorno.DescricaoParecerConclusivoAnoAnterior = parecerConclusivoTurmaAnoAnterior?.DescricaoParecerConclusivo;
                retorno.QdadeBuscasAtivasBimestre = mapeamentoEstudante.ReadFirst<int>();
                retorno.QdadePlanosAEEAtivos = mapeamentoEstudante.ReadFirst<int>();
                retorno.QdadeEncaminhamentosNAAPAAtivos = mapeamentoEstudante.ReadFirst<int>();
                retorno.AnotacoesPedagogicasBimestreAnterior = mapeamentoEstudante.ReadFirstOrDefault<string>();
            }
            return retorno;
        }

        public async Task<string[]> ObterCodigosAlunosComMapeamentoEstudanteBimestre(long turmaId, int bimestre)
        => (await database.Conexao.QueryAsync<string>(@"SELECT distinct(aluno_codigo)  
                                                        FROM mapeamento_estudante
                                                        WHERE turma_id = @turmaId
                                                        AND bimestre = @bimestre", new { turmaId, bimestre })).ToArray();

        public Task<long> ObterTurmaIdMapeamentoEstudante(long mapeamentoEstudanteId)
        => database.Conexao.QueryFirstOrDefaultAsync<long>("SELECT turma_id FROM mapeamento_estudante WHERE id = @mapeamentoEstudanteId ", new { mapeamentoEstudanteId });
        
    }
}

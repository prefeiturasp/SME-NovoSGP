using Dapper;
using Dommel;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Text;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioTurma : IRepositorioTurma
    {
        private const string QuerySincronizacao = @"
                    select
	                    id,
	                    turma_id,
	                    ue_id,
	                    nome,
	                    ano,
	                    ano_letivo,
	                    modalidade_codigo,
	                    semestre,
	                    qt_duracao_aula,
	                    tipo_turno,
	                    data_atualizacao,
                        ensino_especial,
                        etapa_eja,
                        data_inicio
                    from
	                    public.turma
                    where turma_id in (#ids);";

        private const string Update = @"
                    update
	                    public.turma
                    set
	                    nome = @nome,
	                    ano = @ano,
	                    ano_letivo = @anoLetivo,
	                    modalidade_codigo = @modalidadeCodigo,
	                    semestre = @semestre,
	                    qt_duracao_aula = @qtDuracaoAula,
	                    tipo_turno = @tipoTurno,
	                    data_atualizacao = @dataAtualizacao,
                        ensino_especial = @ensinoEspecial,
                        etapa_eja = @etapaEja,
                        data_inicio = @dataInicio
                    where
	                    id = @id;";

        private const string Delete = @"
                    delete from public.compensacao_ausencia_aluno
                    where compensacao_ausencia_id in (select id
                                                      from public.compensacao_ausencia
                                                      where turma_id in (#queryIdsTurmasForaListaCodigos));

                    delete from public.compensacao_ausencia
                    where turma_id in (#queryIdsTurmasForaListaCodigos);

                    delete from public.pendencia_fechamento
                    where fechamento_turma_disciplina_id in (#queryIdsFechamentoTurmaDisciplinaTurmasForaListaCodigos);

                    delete from public.wf_aprovacao_nota_fechamento
                    where fechamento_nota_id in (select id
                                                 from public.fechamento_nota
                                                 where fechamento_aluno_id in (#queryFechamentoAlunoTurmasForaListaCodigos));

                    delete from public.fechamento_nota
                    where fechamento_aluno_id in (#queryFechamentoAlunoTurmasForaListaCodigos);

                    delete from public.fechamento_aluno
                    where fechamento_turma_disciplina_id in (#queryIdsFechamentoTurmaDisciplinaTurmasForaListaCodigos);

                    delete from public.fechamento_turma_disciplina
                    where fechamento_turma_id in (#queryIdsFechamentoTurmaTurmasForaListaCodigos);

                    delete from public.conselho_classe_nota
                    where conselho_classe_aluno_id in (select id
                                                       from public.conselho_classe_aluno
                                                       where conselho_classe_id in (#queryIdsConselhoClasseTurmasForaListaCodigos));

                    delete from public.conselho_classe_aluno
                    where conselho_classe_id in (#queryIdsConselhoClasseTurmasForaListaCodigos);        

                    delete from public.conselho_classe
                    where fechamento_turma_id in (select id
                                                  from public.fechamento_turma
                                                  where turma_id in (#queryIdsTurmasForaListaCodigos));

                    delete from public.fechamento_turma
                    where turma_id in (#queryIdsTurmasForaListaCodigos);

                  
                    delete from public.frequencia_aluno
                    where turma_id in (#codigosTurmasARemover);

                    delete from public.diario_bordo
                    where aula_id in (#queryIdsAulasTurmasForaListaCodigos);

                    delete from public.aula
                    where id in (#queryIdsAulasTurmasForaListaCodigos);
                    
                    delete from public.turma
                    where not historica 
                        and turma_id not in (#ids);";

        private const string QueryIdsTurmasForaListaCodigos = "select id from public.turma where not historica and turma_id not in (#ids)";

        private const string QueryIdsFechamentoTurmaDisciplinaTurmasForaListaCodigos = @"select id
                                                                                         from public.fechamento_turma_disciplina
                                                                                         where fechamento_turma_id in (select id
                                                                                                                       from public.fechamento_turma
                                                                                                                       where turma_id in (#queryIdsTurmasForaListaCodigos))";

        private const string QueryIdsFechamentoTurmaTurmasForaListaCodigos = @"select id
                                                                               from public.fechamento_turma
                                                                               where turma_id in (#queryIdsTurmasForaListaCodigos)";

        private const string QueryIdsConselhoClasseTurmasForaListaCodigos = @"select id
                                                                              from public.conselho_classe
                                                                              where fechamento_turma_id in (#queryIdsFechamentoTurmaTurmasForaListaCodigos)";

        private const string QueryFechamentoAlunoTurmasForaListaCodigos = @"select id
                                                                            from public.fechamento_aluno
                                                                            where fechamento_turma_disciplina_id in (#queryIdsFechamentoTurmaDisciplinaTurmasForaListaCodigos)";

        private const string QueryAulasTurmasForaListaCodigos = @"select id from public.aula where turma_id in (#codigosTurmasARemover)";

        private const string QueryCodigosTurmasForaListaCodigos = "select turma_id from public.turma where not historica and turma_id not in (#ids)";

        private readonly ISgpContext contexto;

        public RepositorioTurma(ISgpContext contexto)
        {
            this.contexto = contexto;
        }

        public IEnumerable<Turma> MaterializarCodigosTurma(string[] idTurmas, out string[] codigosNaoEncontrados)
        {
            List<Turma> resultado = new List<Turma>();
            List<string> naoEncontrados = new List<string>();

            for (int i = 0; i < idTurmas.Count(); i = i + 900)
            {
                var iteracao = idTurmas.Skip(i).Take(900);

                var armazenados = contexto.Conexao.Query<Turma>(QuerySincronizacao.Replace("#ids", string.Join(",", idTurmas.Select(x => $"'{x}'"))));

                naoEncontrados.AddRange(iteracao.Where(x => !armazenados.Select(y => y.CodigoTurma).Contains(x)));

                resultado.AddRange(armazenados);
            }
            codigosNaoEncontrados = naoEncontrados.ToArray();

            return resultado;
        }

        public async Task<Turma> ObterPorCodigo(string turmaCodigo)
        {
            return await contexto.QueryFirstOrDefaultAsync<Turma>("select * from turma where turma_id = @turmaCodigo", new { turmaCodigo });
        }

        public async Task<long> ObterTurmaIdPorCodigo(string turmaCodigo)
        {
            return await contexto.QueryFirstOrDefaultAsync<long>("select id from turma where turma_id = @turmaCodigo", new { turmaCodigo });
        }

        public async Task<Turma> ObterPorId(long id)
        {
            return await contexto.QueryFirstOrDefaultAsync<Turma>("select * from turma where id = @id", new { id });
        }

        public async Task<Turma> ObterTurmaComUeEDrePorCodigo(string turmaCodigo)
        {
            var query = @"select
	                        t.id,
	                        t.turma_id,
	                        t.ue_id,
	                        t.nome,
	                        t.ano,
	                        t.ano_letivo,
	                        t.modalidade_codigo,
	                        t.semestre,
	                        t.qt_duracao_aula,
	                        t.tipo_turno,
	                        t.data_atualizacao,
	                        u.id as UeId,
	                        u.id,
	                        u.ue_id,
	                        u.nome,
	                        u.dre_id,
	                        u.tipo_escola,
	                        u.data_atualizacao,
	                        d.id as DreId,
	                        d.id,
	                        d.nome,
	                        d.dre_id,
	                        d.abreviacao,
	                        d.data_atualizacao

                        from
	                        turma t
                        inner join ue u on
	                        t.ue_id = u.id
                        inner join dre d on
	                        u.dre_id = d.id
                        where
	                        turma_id = @turmaCodigo";
            return (await contexto.QueryAsync<Turma, Ue, Dre, Turma>(query, (turma, ue, dre) =>
             {
                 ue.AdicionarDre(dre);
                 turma.AdicionarUe(ue);
                 return turma;
             }, new { turmaCodigo }, splitOn: "TurmaId, UeId, DreId")).FirstOrDefault();
        }

        public async Task<Turma> ObterTurmaComUeEDrePorId(long turmaId)
        {
            var query = @"select
	                        t.id,
	                        t.turma_id,
	                        t.ue_id,
	                        t.nome,
	                        t.ano,
	                        t.ano_letivo,
	                        t.modalidade_codigo,
	                        t.semestre,
	                        t.qt_duracao_aula,
	                        t.tipo_turno,
	                        t.data_atualizacao,
	                        u.id as UeId,
	                        u.id,
	                        u.ue_id,
	                        u.nome,
	                        u.dre_id,
	                        u.tipo_escola,
	                        u.data_atualizacao,
	                        d.id as DreId,
	                        d.id,
	                        d.nome,
	                        d.dre_id,
	                        d.abreviacao,
	                        d.data_atualizacao
                        from
	                        turma t
                        inner join ue u on
	                        t.ue_id = u.id
                        inner join dre d on
	                        u.dre_id = d.id
                        where
	                        t.id = @turmaId";
            return (await contexto.QueryAsync<Turma, Ue, Dre, Turma>(query, (turma, ue, dre) =>
            {
                ue.AdicionarDre(dre);
                turma.AdicionarUe(ue);
                return turma;
            }, new { turmaId }, splitOn: "TurmaId, UeId, DreId")).FirstOrDefault();
        }

        public async Task<bool> ObterTurmaEspecialPorCodigo(string turmaCodigo)
        {
            var query = "select ensino_especial from turma where turma_id = @turmaCodigo";

            return await contexto.Conexao.QueryFirstAsync<bool>(query, new { turmaCodigo });
        }

        public async Task<IEnumerable<long>> ObterTurmasPorUeAnos(string ueCodigo, int anoLetivo, string[] anos, int modalidadeId)
        {
            var query = new StringBuilder(@"select x.turma_Id from (select distinct on (2) t.turma_id , t.ano from turma t
                                                inner join ue u on u.id  = t.ue_id 
                                            where  t.ano_letivo = @anoLetivo and t.modalidade_codigo = @modalidadeId ");

            if (!string.IsNullOrEmpty(ueCodigo))
                query.AppendLine("and u.ue_id  = @ueCodigo");

            if (anos != null && anos.Length > 0)
                query.AppendLine("and t.ano = any(@anos) ");

            query.AppendLine(") x");

            return await contexto.Conexao.QueryAsync<long>(query.ToString(), new { ueCodigo, anos, anoLetivo, modalidadeId });

        }

        public async Task<IEnumerable<Turma>> SincronizarAsync(IEnumerable<Turma> entidades, IEnumerable<Ue> ues)
        {
            List<Turma> resultado = new List<Turma>();

            await RemoverTurmasExtintasAsync(entidades);

            for (int i = 0; i < entidades.Count(); i = i + 900)
            {
                var iteracao = entidades.Skip(i).Take(900);

                var armazenados = (await contexto.Conexao.QueryAsync<Turma>(
                    QuerySincronizacao.Replace("#ids", string.Join(",", iteracao.Select(x => $"'{x.CodigoTurma}'"))))).ToList();

                var idsArmazenados = armazenados.Select(y => y.CodigoTurma);
                var novos = iteracao.Where(x => !idsArmazenados.Contains(x.CodigoTurma)).ToList();

                foreach (var item in novos)
                {
                    item.DataAtualizacao = DateTime.Today;
                    item.Ue = ues.First(x => x.CodigoUe == item.Ue.CodigoUe);
                    item.UeId = item.Ue.Id;
                    item.Id = (long)await contexto.Conexao.InsertAsync(item);
                    resultado.Add(item);
                }

                var modificados = from c in entidades
                                  join l in armazenados on c.CodigoTurma equals l.CodigoTurma
                                  where l.DataAtualizacao != DateTime.Today &&
                                        (c.Nome != l.Nome ||
                                        c.Ano != l.Ano ||
                                        c.AnoLetivo != l.AnoLetivo ||
                                        c.ModalidadeCodigo != l.ModalidadeCodigo ||
                                        c.Semestre != l.Semestre ||
                                        c.QuantidadeDuracaoAula != l.QuantidadeDuracaoAula ||
                                        c.TipoTurno != l.TipoTurno ||
                                        c.EnsinoEspecial != l.EnsinoEspecial ||
                                        c.EtapaEJA != l.EtapaEJA ||
                                        c.DataInicio != l.DataInicio)
                                  select new Turma()
                                  {
                                      Ano = c.Ano,
                                      AnoLetivo = c.AnoLetivo,
                                      CodigoTurma = c.CodigoTurma,
                                      DataAtualizacao = DateTime.Today,
                                      Id = l.Id,
                                      ModalidadeCodigo = c.ModalidadeCodigo,
                                      Nome = c.Nome,
                                      QuantidadeDuracaoAula = c.QuantidadeDuracaoAula,
                                      Semestre = c.Semestre,
                                      TipoTurno = c.TipoTurno,
                                      Ue = l.Ue,
                                      UeId = l.UeId,
                                      EnsinoEspecial = c.EnsinoEspecial,
                                      EtapaEJA = c.EtapaEJA,
                                      DataInicio = c.DataInicio
                                  };

                foreach (var item in modificados)
                {
                    await contexto.Conexao.ExecuteAsync(Update, new
                    {
                        nome = item.Nome,
                        ano = item.Ano,
                        anoLetivo = item.AnoLetivo,
                        modalidadeCodigo = item.ModalidadeCodigo,
                        semestre = item.Semestre,
                        qtDuracaoAula = item.QuantidadeDuracaoAula,
                        tipoTurno = item.TipoTurno,
                        dataAtualizacao = item.DataAtualizacao,
                        id = item.Id,
                        ensinoEspecial = item.EnsinoEspecial,
                        etapaEja = item.EtapaEJA,
                        dataInicio = item.DataInicio
                    });

                    resultado.Add(item);
                }

                resultado.AddRange(armazenados.Where(x => !resultado.Select(y => y.CodigoTurma).Contains(x.CodigoTurma)));
            }

            return resultado;
        }

        public async Task<IEnumerable<Turma>> ObterTurmasInfantilNaoDeProgramaPorAnoLetivoAsync(int anoLetivo)
        {
            var modalidade = Modalidade.Infantil;
            var turmas = new List<Turma>();
            var query = @"select
	                            t.*,
	                            u.*,
                                d.*
                            from
	                            turma t
                            inner join ue u on
	                            u.id = t.ue_id
                            inner join dre d on
	                            u.dre_id = d.id
                            where
	                            t.modalidade_codigo = :modalidade
	                            and t.historica = false
	                            and t.ano_letivo = :anoLetivo
	                            and ano ~ E'^[0-9\.]+$'";

            await contexto.Conexao.QueryAsync<Turma, Ue, Dre, Turma>(query, (turma, ue, dre) =>
             {
                 ue.AdicionarDre(dre);
                 turma.AdicionarUe(ue);

                 var turmaExistente = turmas.FirstOrDefault(c => c.Id == turma.Id);
                 if (turmaExistente == null)
                     turmas.Add(turma);

                 return turma;
             }, new { anoLetivo, modalidade });

            return turmas;
        }

        private async Task RemoverTurmasExtintasAsync(IEnumerable<Turma> entidades)
        {
            var codigosTurmas = entidades.Select(e => $"'{e.CodigoTurma}'")?.ToArray();
            var listaTurmas = string.Join(",", codigosTurmas);

            var sql = Delete.Replace("#queryIdsConselhoClasseTurmasForaListaCodigos", QueryIdsConselhoClasseTurmasForaListaCodigos)
                                .Replace("#queryFechamentoAlunoTurmasForaListaCodigos", QueryFechamentoAlunoTurmasForaListaCodigos)
                                .Replace("#queryIdsFechamentoTurmaTurmasForaListaCodigos", QueryIdsFechamentoTurmaTurmasForaListaCodigos)
                                .Replace("#queryIdsFechamentoTurmaDisciplinaTurmasForaListaCodigos", QueryIdsFechamentoTurmaDisciplinaTurmasForaListaCodigos)
                                .Replace("#queryIdsTurmasForaListaCodigos", QueryIdsTurmasForaListaCodigos)
                                .Replace("#queryIdsAulasTurmasForaListaCodigos", QueryAulasTurmasForaListaCodigos)
                                .Replace("#queryCodigosTurmasForaListaCodigos", QueryCodigosTurmasForaListaCodigos)
                                .Replace("#codigosTurmasARemover", QueryCodigosTurmasForaListaCodigos)
                                .Replace("#ids", listaTurmas);
            await contexto.Conexao
                .ExecuteAsync(sql);
        }
    }
}

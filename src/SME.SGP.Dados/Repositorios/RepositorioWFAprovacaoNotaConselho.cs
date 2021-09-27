using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Dados
{
    public class RepositorioWFAprovacaoNotaConselho : IRepositorioWFAprovacaoNotaConselho
    {
        protected readonly ISgpContext database;

        public RepositorioWFAprovacaoNotaConselho(ISgpContext database)
        {
            this.database = database ?? throw new ArgumentNullException(nameof(database));
        }

        public async Task Excluir(long id)
        {
            var query = @"delete from wf_aprovacao_nota_conselho where id = @id";

            await database.Conexao.ExecuteScalarAsync(query, new { id });
        }

        public async Task<WFAprovacaoNotaConselho> ObterNotaEmAprovacao(long workflowId)
        {
            var query = @"select nwf.*
	                    , ccn.*
	                    , cca.*
	                    , cc.*
	                    , ft.*
	                    , t.*
                      from wf_aprovacao_nota_conselho nwf
                     inner join conselho_classe_nota ccn on ccn.id = nwf.conselho_classe_nota_id
                     inner join conselho_classe_aluno cca on cca.id = ccn.conselho_classe_aluno_id 
                     inner join conselho_classe cc on cc.id = cca.conselho_classe_id 
                     inner join fechamento_turma ft on ft.id = cc.fechamento_turma_id 
                     inner join turma t on t.id = ft.turma_id 
                     where nwf.wf_aprovacao_id = @workflowId";

            return (await database.Conexao
                .QueryAsync<WFAprovacaoNotaConselho, ConselhoClasseNota, ConselhoClasseAluno, ConselhoClasse, FechamentoTurma, Turma, WFAprovacaoNotaConselho>(query,
                (wfAprovacao, conselhoClasseNota, conselhoClasseAluno, conselhoClasse, fechamentoTurma, turma) => 
                {
                    fechamentoTurma.Turma = turma;
                    conselhoClasse.FechamentoTurma = fechamentoTurma;
                    conselhoClasseAluno.ConselhoClasse = conselhoClasse;
                    conselhoClasseNota.ConselhoClasseAluno = conselhoClasseAluno;
                    wfAprovacao.ConselhoClasseNota = conselhoClasseNota;

                    return wfAprovacao;
                }
                , new { workflowId }))
                .FirstOrDefault();
        }

        public async Task Salvar(WFAprovacaoNotaConselho entidade)
        {
            if (entidade.Id > 0)
                await database.Conexao.UpdateAsync(entidade);
            else
                await database.Conexao.InsertAsync(entidade);
        }

    }
}

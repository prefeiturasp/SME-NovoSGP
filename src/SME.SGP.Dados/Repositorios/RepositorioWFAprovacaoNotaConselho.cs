using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Dados
{
    public class RepositorioWFAprovacaoNotaConselho : RepositorioBase<WFAprovacaoNotaConselho>, IRepositorioWFAprovacaoNotaConselho
    {

        public RepositorioWFAprovacaoNotaConselho(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public async Task<WFAprovacaoNotaConselho> ObterNotaEmAprovacaoPorWorkflow(long workflowId)
        {
            var query = @"select nwf.*
	                    , ccn.*
	                    , cca.*
	                    , cc.*
	                    , ft.*
	                    , t.*
                        , pe.*
                      from wf_aprovacao_nota_conselho nwf
                     inner join conselho_classe_nota ccn on ccn.id = nwf.conselho_classe_nota_id
                     inner join conselho_classe_aluno cca on cca.id = ccn.conselho_classe_aluno_id 
                     inner join conselho_classe cc on cc.id = cca.conselho_classe_id 
                     inner join fechamento_turma ft on ft.id = cc.fechamento_turma_id 
                     inner join turma t on t.id = ft.turma_id 
                     inner join periodo_escolar pe on ft.periodo_escolar_id = pe.id 
                     where nwf.wf_aprovacao_id = @workflowId";

            return (await database.Conexao
                .QueryAsync<WFAprovacaoNotaConselho, ConselhoClasseNota, ConselhoClasseAluno, ConselhoClasse, FechamentoTurma, Turma, PeriodoEscolar, WFAprovacaoNotaConselho>(query,
                (wfAprovacao, conselhoClasseNota, conselhoClasseAluno, conselhoClasse, fechamentoTurma, turma, periodoEscolar) => 
                {
                    fechamentoTurma.Turma = turma;
                    conselhoClasse.FechamentoTurma = fechamentoTurma;
                    conselhoClasse.FechamentoTurma.PeriodoEscolar = periodoEscolar;
                    conselhoClasseAluno.ConselhoClasse = conselhoClasse;
                    conselhoClasseNota.ConselhoClasseAluno = conselhoClasseAluno;
                    wfAprovacao.ConselhoClasseNota = conselhoClasseNota;

                    return wfAprovacao;
                }
                , new { workflowId }))
                .FirstOrDefault();
        }

        public async Task<IEnumerable<WFAprovacaoNotaConselho>> ObterWorkflowAprovacaoNota(long conselhoClasseNotaId)
        {
            var query = @"select * from wf_aprovacao_nota_conselho where conselho_classe_nota_id = @conselhoClasseNotaId";

            return await database.Conexao.QueryAsync<WFAprovacaoNotaConselho>(query, new { conselhoClasseNotaId });
        }
    }
}

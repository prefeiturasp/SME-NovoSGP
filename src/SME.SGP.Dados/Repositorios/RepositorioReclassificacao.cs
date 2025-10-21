using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.PainelEducacional.Reclassificacao;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using System.Linq;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioReclassificacao : IRepositorioReclassificacao
    {
        private readonly ISgpContext database;
        public RepositorioReclassificacao(ISgpContext database)
        {
            this.database = database;
        }

        public async Task<IEnumerable<PainelEducacionalReclassificacaoDto>> ObterReclassificacao(string codigoDre, string codigoUe, int anoLetivo, string anoTurma)
        {
            string query = @"
                select 
                    modalidade_turma as Nome,
                    ano_turma as AnoTurma,
                    quantidade_alunos_reclassificados as QuantidadeAlunos
                from painel_educacional_consolidacao_reclassificacao
                where 1 = 1 ";

            if (!string.IsNullOrWhiteSpace(codigoDre))
                query += " AND codigo_dre = @codigoDre ";

            if (!string.IsNullOrWhiteSpace(codigoUe))
                query += " AND codigo_ue = @codigoUe ";

            if (anoLetivo > 0)
                query += " AND ano_letivo = @anoLetivo ";

            if (!string.IsNullOrWhiteSpace(anoTurma))
                query += " AND ano_turma = @anoTurma ";

            var modalidades = await database.Conexao.QueryAsync<ModalidadeReclassificacaoDto>(query, new
            {
                codigoDre,
                codigoUe,
                anoLetivo,
                anoTurma
            });

            if (modalidades?.Any() == true)
            {
                return new List<PainelEducacionalReclassificacaoDto>
                {
                    new PainelEducacionalReclassificacaoDto
                    {
                        Modalidade = modalidades.Select(m => new ModalidadeReclassificacaoDto
                        {
                            Nome = ObterNomeModalidade(m.Nome),
                            AnoTurma = m.AnoTurma,
                            QuantidadeAlunos = m.QuantidadeAlunos
                        })
                    }
                };
            }

            return Enumerable.Empty<PainelEducacionalReclassificacaoDto>();
        }

        private string ObterNomeModalidade(string codigoModalidade)
        {
            return codigoModalidade switch
            {
                "3" => "EJA",
                "5" => "Ensino Fundamental",
                "6" => "Ensino Médio",
                _ => codigoModalidade
            };
        }
    }
}

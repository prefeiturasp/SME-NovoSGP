using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioCorrelacaoInforme : RepositorioBase<InformeCorrelacao>, IRepositorioCorrelacaoInforme
    {
        private readonly ISgpContext contexto;

        public RepositorioCorrelacaoInforme(ISgpContext contexto, IServicoAuditoria servicoAuditoria) : base(contexto, servicoAuditoria)
        {
            this.contexto = contexto;
        }

        public InformeCorrelacao ObterPorCodigoCorrelacao(Guid codigoCorrelacao)
        {
            return contexto.Conexao.QueryFirstOrDefault<InformeCorrelacao>("select * from informativo_correlacao where codigo = @codigoCorrelacao", new { codigoCorrelacao });
        }
      
        public async Task<DataCriacaoInformeDto> ObterDataCriacaoInforme(Guid codigoCorrelacao)
        {
            return await contexto.Conexao.QueryFirstOrDefaultAsync<DataCriacaoInformeDto>("SELECT criado_em AS CriadoEm FROM informativo_correlacao rc WHERE codigo = @codigoCorrelacao", new { codigoCorrelacao });
        }

        public async Task<InformeCorrelacao> ObterPorCodigoCorrelacaoAsync(Guid codigoCorrelacao)
        {
            return await contexto.Conexao.QueryFirstOrDefaultAsync<InformeCorrelacao>("select * from informativo_correlacao where codigo = @codigoCorrelacao", new { codigoCorrelacao });
        }
    }
}
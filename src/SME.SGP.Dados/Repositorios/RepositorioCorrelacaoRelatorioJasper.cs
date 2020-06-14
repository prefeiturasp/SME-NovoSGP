using Dommel;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioRelatorioCorrelacaoJasper : IRepositorioCorrelacaoRelatorioJasper
    {
        private readonly ISgpContext contexto;

        public RepositorioRelatorioCorrelacaoJasper(ISgpContext contexto)
        {
            this.contexto = contexto;
        }

        public long Salvar(RelatorioCorrelacaoJasper relatorioCorrelacaoJasper)
        {
            if (relatorioCorrelacaoJasper.Id > 0)
            {
                contexto.Conexao.Update(relatorioCorrelacaoJasper);
            }
            else
            {
                relatorioCorrelacaoJasper.Id = (long)(contexto.Conexao.Insert(relatorioCorrelacaoJasper));
            }

            return relatorioCorrelacaoJasper.Id;
        }
    }
}
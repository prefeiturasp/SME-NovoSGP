using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioRelatorioCorrelacaoJasper : RepositorioBase<RelatorioCorrelacaoJasper>, IRepositorioRelatorioCorrelacaoJasper
    {
        private readonly ISgpContext contexto;

        public RepositorioRelatorioCorrelacaoJasper(ISgpContext contexto) : base(contexto)
        {
            this.contexto = contexto;
        }
    }
}
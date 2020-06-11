using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioCorrelacaoRelatorio : RepositorioBase<RelatorioCorrelacao>, IRepositorioCorrelacaoRelatorio
    {
        private readonly ISgpContext contexto;

        public RepositorioCorrelacaoRelatorio(ISgpContext contexto) : base(contexto)
        {
            this.contexto = contexto;
        }
    }
}
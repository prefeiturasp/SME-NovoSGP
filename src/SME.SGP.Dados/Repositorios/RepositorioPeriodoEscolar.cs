using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Interfaces;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPeriodoEscolar : RepositorioBase<PeriodoEscolar>, IRepositorioPeriodoEscolar
    {
        public RepositorioPeriodoEscolar(ISgpContextConsultas conexao) : base(conexao) { }        
    }
}
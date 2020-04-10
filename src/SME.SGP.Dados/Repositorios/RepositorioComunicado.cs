using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioComunicado : RepositorioBase<Comunicado>, IRepositorioComunicado
    {
        public RepositorioComunicado(ISgpContext conexao) : base(conexao)
        {
        }
    }
}
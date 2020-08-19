using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioDiarioBordoObservacao : RepositorioBase<DiarioBordoObservacao>, IRepositorioDiarioBordoObservacao
    {
        public RepositorioDiarioBordoObservacao(ISgpContext conexao) : base(conexao) { }
    }
}

using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioObjetivoDesenvolvimento : RepositorioBase<ObjetivoDesenvolvimento>, IRepositorioObjetivoDesenvolvimento
    {
        public RepositorioObjetivoDesenvolvimento(ISgpContext conexao) : base(conexao)
        {
        }
    }
}
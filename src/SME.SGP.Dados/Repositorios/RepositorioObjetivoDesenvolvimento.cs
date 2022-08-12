using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioObjetivoDesenvolvimento : RepositorioBase<RecuperacaoParalelaObjetivoDesenvolvimento>, IRepositorioObjetivoDesenvolvimento
    {
        public RepositorioObjetivoDesenvolvimento(ISgpContext conexao) : base(conexao)
        {
        }
    }
}
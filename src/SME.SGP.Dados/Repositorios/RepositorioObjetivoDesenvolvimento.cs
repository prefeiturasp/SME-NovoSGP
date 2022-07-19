using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioObjetivoDesenvolvimento : RepositorioBase<RecuperacaoParalelaObjetivoDesenvolvimento>, IRepositorioObjetivoDesenvolvimento
    {
        public RepositorioObjetivoDesenvolvimento(ISgpContext conexao, IServicoAuditoria servicoAuditoria) : base(conexao, servicoAuditoria)
        {
        }
    }
}
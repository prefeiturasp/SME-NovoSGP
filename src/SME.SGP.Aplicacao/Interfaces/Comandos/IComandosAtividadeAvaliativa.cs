using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IComandosAtividadeAvaliativa
    {
        Task<IEnumerable<RetornoCopiarAtividadeAvaliativaDto>> Alterar(AtividadeAvaliativaDto dto, long id);

        Task Excluir(long idAtividadeAvaliativa);

        Task<IEnumerable<RetornoCopiarAtividadeAvaliativaDto>> Inserir(AtividadeAvaliativaDto dto);

        Task Validar(FiltroAtividadeAvaliativaDto filtro);
    }
}
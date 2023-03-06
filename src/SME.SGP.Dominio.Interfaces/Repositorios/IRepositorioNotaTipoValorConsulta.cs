using SME.SGP.Dominio.Enumerados;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioNotaTipoValorConsulta : IRepositorioBase<NotaTipoValor>
    {
        Task<NotaTipoValor> ObterPorCicloIdDataAvalicacao(long cicloId, DateTime dataAvalicao);

        NotaTipoValor ObterPorTurmaId(long turmaId, TipoTurma tipoTurma = TipoTurma.Regular);

        Task<NotaTipoValor> ObterPorTurmaIdAsync(long turmaId, TipoTurma tipoTurma = TipoTurma.Regular);
    }
}
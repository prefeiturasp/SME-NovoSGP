using SME.SGP.Dominio.Enumerados;
using System;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioNotaTipoValor : IRepositorioBase<NotaTipoValor>
    {
        NotaTipoValor ObterPorCicloIdDataAvalicacao(long cicloId, DateTime dataAvalicao);

        NotaTipoValor ObterPorTurmaId(long turmaId, TipoTurma tipoTurma = TipoTurma.Regular);
    }
}
using System;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioNotaTipoValor : IRepositorioBase<NotaTipoValor>
    {
        NotaTipoValor ObterPorCicloIdDataAvalicacao(long cicloId, DateTime dataAvalicao);

        NotaTipoValor ObterPorTurmaId(long turmaId);
    }
}
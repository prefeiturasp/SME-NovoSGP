﻿using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioOpcaoResposta : IRepositorioBase<OpcaoResposta>
    {
        Task<IEnumerable<OpcaoRespostaSimplesDto>> ObterOpcoesRespostaPorQuestaoId(long QuestaoId);
    }
}

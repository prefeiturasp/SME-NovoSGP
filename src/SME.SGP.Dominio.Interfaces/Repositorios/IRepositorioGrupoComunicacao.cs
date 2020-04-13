﻿using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioGrupoComunicacao : IRepositorioBase<GrupoComunicacao>
    {
        Task<IEnumerable<GrupoComunicacaoCompletoRespostaDto>> Listar(FiltroGrupoComunicacaoDto filtro);

        Task<IEnumerable<GrupoComunicacaoCompletoRespostaDto>> ObterPorIdAsync(long id);
    }
}
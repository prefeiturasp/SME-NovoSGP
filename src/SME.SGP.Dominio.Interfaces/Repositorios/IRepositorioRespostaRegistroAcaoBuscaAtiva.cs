﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioRespostaRegistroAcaoBuscaAtiva : IRepositorioBase<RespostaRegistroAcaoBuscaAtiva>
    {
        Task<IEnumerable<RespostaRegistroAcaoBuscaAtiva>> ObterPorQuestaoRegistroAcaoId(long questaoRegistroAcaoId);
    }
}

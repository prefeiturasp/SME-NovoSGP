﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioRespostaEncaminhamentoNAAPA : IRepositorioBase<RespostaEncaminhamentoNAAPA>
    {
        Task<bool> RemoverPorArquivoId(long arquivoId);
        Task<IEnumerable<RespostaEncaminhamentoNAAPA>> ObterPorQuestaoEncaminhamentoId(long requestQuestaoEncaminhamentoNaapaId);
        Task<IEnumerable<long>> ObterArquivosPorQuestaoId(long questaoEncaminhamentoAEEId);
    }
}

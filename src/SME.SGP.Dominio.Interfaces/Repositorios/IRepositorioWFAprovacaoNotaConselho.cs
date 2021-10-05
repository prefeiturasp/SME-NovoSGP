﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioWFAprovacaoNotaConselho
    {
        Task Salvar(WFAprovacaoNotaConselho entidade);
        Task<WFAprovacaoNotaConselho> ObterNotaEmAprovacaoPorWorkflow(long workflowId);
        Task<IEnumerable<WFAprovacaoNotaConselho>> ObterWorkflowAprovacaoNota(long conselhoClasseNotaId);
        Task Excluir(long id);
    }
}

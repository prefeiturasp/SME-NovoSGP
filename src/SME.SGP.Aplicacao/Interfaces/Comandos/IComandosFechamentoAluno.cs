using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public interface IComandosFechamentoAluno
    {
        Task<AuditoriaPersistenciaDto> SalvarAnotacaoAluno(AnotacaoAlunoDto anotacaoAluno);
    }
}

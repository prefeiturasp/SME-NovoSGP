using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasAnotacaoAlunoFechamento
    {
        Task<AnotacaoAlunoCompletoDto> ObterAnotacaoAluno(string codigoAluno, long fechamentoId, string codigoTurma, int anoLetivo);
        Task<AnotacaoAlunoFechamento> ObterAnotacaoPorAlunoEFechamento(long fechamentoId, string codigoAluno);
    }
}

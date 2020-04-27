using SME.SGP.Dominio;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultasRelatorioSemestralAluno : ConsultasBase, IConsultasRelatorioSemestralAluno
    {
        public ConsultasRelatorioSemestralAluno(IContextoAplicacao contextoAplicacao): base(contextoAplicacao)
        {

        }

        public Task<RelatorioSemestralAluno> ObterPorTurmaAlunoAsync(long relatorioSemestralId, string alunoCodigo)
        {
            throw new NotImplementedException();
        }
    }
}

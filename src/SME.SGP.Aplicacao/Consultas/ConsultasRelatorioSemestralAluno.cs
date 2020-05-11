using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace SME.SGP.Aplicacao
{
    public class ConsultasRelatorioSemestralAluno : ConsultasBase, IConsultasRelatorioSemestralAluno
    {
        private readonly IConsultasFechamentoTurmaDisciplina consultasFechamentoTurmaDisciplina;

        public ConsultasRelatorioSemestralAluno(IContextoAplicacao contextoAplicacao,
                                                IConsultasFechamentoTurmaDisciplina consultasFechamentoTurmaDisciplina) : base(contextoAplicacao)
        {
            this.consultasFechamentoTurmaDisciplina = consultasFechamentoTurmaDisciplina ?? throw new ArgumentNullException(nameof(consultasFechamentoTurmaDisciplina));
        }

        public async Task<IEnumerable<AlunoDadosBasicosDto>> ObterListaAlunosAsync(string turmaCodigo, int anoLetivo, int semestre)
        {
            var listaAlunos = await consultasFechamentoTurmaDisciplina.ObterDadosAlunos(turmaCodigo, anoLetivo, semestre);
            return listaAlunos;
        }

        public Task<RelatorioSemestralAluno> ObterPorTurmaAlunoAsync(long relatorioSemestralId, string alunoCodigo)
        {
            throw new NotImplementedException();
        }
    }
}

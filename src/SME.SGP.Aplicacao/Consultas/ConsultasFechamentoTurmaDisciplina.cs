using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultasFechamentoTurmaDisciplina : IConsultasFechamentoTurmaDisciplina
    {
        private readonly IRepositorioFechamentoTurmaDisciplina repositorioFechamentoTurmaDisciplina;
        public ConsultasFechamentoTurmaDisciplina(IRepositorioFechamentoTurmaDisciplina repositorioFechamentoTurmaDisciplina)
        {
            this.repositorioFechamentoTurmaDisciplina = repositorioFechamentoTurmaDisciplina ?? throw new ArgumentNullException(nameof(repositorioFechamentoTurmaDisciplina));
        }

        public async Task<FechamentoTurmaDisciplina> ObterFechamentoTurmaDisciplina(string turmaId, long disciplinaId, int bimestre)
            => await repositorioFechamentoTurmaDisciplina.ObterFechamentoTurmaDisciplina(turmaId, disciplinaId, bimestre);

        public async Task<IEnumerable<NotaConceitoBimestreDto>> ObterNotasBimestre(string codigoAluno, long fechamentoTurmaId)
            => await repositorioFechamentoTurmaDisciplina.ObterNotasBimestre(codigoAluno, fechamentoTurmaId);
    }
}
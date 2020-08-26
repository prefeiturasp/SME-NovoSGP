using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Aplicacao
{
    public class ConsultasRelatorioSemestralTurmaPAP : IConsultasRelatorioSemestralTurmaPAP
    {
        private readonly IRepositorioRelatorioSemestralTurmaPAP repositorioRelatorioSemestral;
        public ConsultasRelatorioSemestralTurmaPAP(IRepositorioRelatorioSemestralTurmaPAP repositorioRelatorioSemestral)
        {
            this.repositorioRelatorioSemestral = repositorioRelatorioSemestral ?? throw new ArgumentNullException(nameof(repositorioRelatorioSemestral));
        }

        public async Task<RelatorioSemestralTurmaPAP> ObterPorIdAsync(long relatorioSemestralId)
            => await repositorioRelatorioSemestral.ObterPorIdAsync(relatorioSemestralId);

        public Task<RelatorioSemestralTurmaPAP> ObterPorTurmaCodigoSemestreAsync(string turmaCodigo, int semestre)
        {
            throw new NotImplementedException();
        }
    }
}

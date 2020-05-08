using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Aplicacao
{
    public class ConsultasRelatorioSemestral : IConsultasRelatorioSemestral
    {
        private readonly IRepositorioRelatorioSemestral repositorioRelatorioSemestral;
        public ConsultasRelatorioSemestral(IRepositorioRelatorioSemestral repositorioRelatorioSemestral)
        {
            this.repositorioRelatorioSemestral = repositorioRelatorioSemestral ?? throw new ArgumentNullException(nameof(repositorioRelatorioSemestral));
        }

        public async Task<RelatorioSemestral> ObterPorIdAsync(long relatorioSemestralId)
            => await repositorioRelatorioSemestral.ObterPorIdAsync(relatorioSemestralId);

        public Task<RelatorioSemestral> ObterPorTurmaCodigoSemestreAsync(string turmaCodigo, int semestre)
        {
            throw new NotImplementedException();
        }
    }
}

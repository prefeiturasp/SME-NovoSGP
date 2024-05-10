using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultasFechamentoNota : IConsultasFechamentoNota
    {
        private readonly IRepositorioFechamentoNotaConsulta repositorioFechamentoNota;
        public ConsultasFechamentoNota(IRepositorioFechamentoNotaConsulta repositorioFechamentoNota)
        {
            this.repositorioFechamentoNota = repositorioFechamentoNota ?? throw new ArgumentNullException(nameof(repositorioFechamentoNota));
        }

        public async Task<IEnumerable<NotaConceitoBimestreComponenteDto>> ObterNotasAlunoBimestreAsync(long fechamentoTurmaId, string alunoCodigo)
            => await repositorioFechamentoNota.ObterNotasAlunoBimestreAsync(fechamentoTurmaId, alunoCodigo);

        public async Task<IEnumerable<NotaConceitoBimestreComponenteDto>> ObterNotasAlunoAnoAsync(string turmaCodigo, string alunoCodigo)
            => await repositorioFechamentoNota.ObterNotasAlunoAnoAsync(turmaCodigo, alunoCodigo);
    }
}
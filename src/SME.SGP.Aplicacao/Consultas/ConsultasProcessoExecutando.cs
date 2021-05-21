using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultasProcessoExecutando : IConsultasProcessoExecutando
    {
        private readonly IRepositorioProcessoExecutando repositorio;

        public ConsultasProcessoExecutando(IRepositorioProcessoExecutando repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<bool> ExecutandoCalculoFrequencia(string turmaId, string disciplinaId, int bimestre)
            => (await repositorio.ObterProcessoCalculoFrequenciaAsync(turmaId, disciplinaId, bimestre, Dominio.TipoProcesso.CalculoFrequencia)) != null;

        public async Task<ProcessoExecutando> ObterProcessosEmExecucaoAsync(string turmaId, string disciplinaId, int bimestre, TipoProcesso tipoProcesso)
            => (await repositorio.ObterProcessoCalculoFrequenciaAsync(turmaId, disciplinaId, bimestre, TipoProcesso.CalculoFrequencia));
    }
}

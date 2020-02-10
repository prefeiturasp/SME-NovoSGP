using SME.SGP.Aplicacao.Interfaces;
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
            => (await repositorio.ObterProcessoCalculoFrequencia(turmaId, disciplinaId, bimestre)) != null;
    }
}

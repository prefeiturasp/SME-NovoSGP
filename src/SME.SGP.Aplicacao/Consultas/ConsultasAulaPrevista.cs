using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultasAulaPrevista : IConsultasAulaPrevista
    {
        private readonly IRepositorioAulaPrevista repositorio;

        public ConsultasAulaPrevista(IRepositorioAulaPrevista repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<IEnumerable<AulasPrevistasDadasDto>> ObterAulaPrevistaDada(string turmaId, string disciplinaId)
        {
            return await repositorio.ObterAulaPrevistaDada(turmaId, disciplinaId);
        }
    }
}

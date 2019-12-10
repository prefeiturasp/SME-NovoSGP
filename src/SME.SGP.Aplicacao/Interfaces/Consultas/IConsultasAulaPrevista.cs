using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IConsultasAulaPrevista
    {
        Task<AulasPrevistasDadasAuditoriaDto> ObterAulaPrevistaDada(Modalidade modalidade, string turmaId, string disciplinaId);
    }
}

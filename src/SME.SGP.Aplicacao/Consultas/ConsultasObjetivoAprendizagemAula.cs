using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Aplicacao
{
    public class ConsultasObjetivoAprendizagemAula : IConsultasObjetivoAprendizagemAula
    {
        private readonly IRepositorioObjetivoAprendizagemAula repositorio;

        public ConsultasObjetivoAprendizagemAula(IRepositorioObjetivoAprendizagemAula repositorioObjetivoAula)
        {
            this.repositorio = repositorioObjetivoAula;
        }

        public async Task<IEnumerable<ObjetivoAprendizagemAula>> ObterObjetivosPlanoAula(long planoAulaId)
        {
            return await repositorio.ObterObjetivosPlanoAula(planoAulaId);
        }
    }
}

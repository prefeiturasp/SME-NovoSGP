﻿using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioItineranciaAlunoQuestao : RepositorioBase<ItineranciaAlunoQuestao>, IRepositorioItineranciaAlunoQuestao
    {
        public RepositorioItineranciaAlunoQuestao(ISgpContext database) : base(database)
        {
        }
    }
}

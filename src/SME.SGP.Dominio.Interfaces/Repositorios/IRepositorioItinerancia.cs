﻿using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioItinerancia : IRepositorioBase<Itinerancia>
    {
        Task<IEnumerable<ItineranciaObjetivosBaseDto>> ObterObjetivosBase();
        Task<IEnumerable<ItineranciaAlunoQuestaoDto>> ObterQuestoesItineranciaAluno(long id);
        Task<IEnumerable<ItineranciaQuestaoBaseDto>> ObterItineranciaQuestaoBase(long[] tiposQuestionario);
        Task<ItineranciaDto> ObterItineranciaPorId(long id);
        Task<IEnumerable<ItineranciaAlunoDto>> ObterItineranciaAlunoPorId(long id);
        Task<IEnumerable<ItineranciaObjetivoDto>> ObterObjetivosItineranciaPorId(long id);
        Task<IEnumerable<ItineranciaQuestaoDto>> ObterQuestoesItineranciaPorId(long id, long tipoQuestionario);
        Task<IEnumerable<ItineranciaUeDto>> ObterUesItineranciaPorId(long id);
        Task<IEnumerable<ItineranciaIdUeInfosDto>> ObterUesItineranciaPorIds(long[] itineranciaIds);
        Task<IEnumerable<ItineranciaNomeRfCriadorRetornoDto>> ObterRfsCriadoresPorNome(string nomeParaBusca);
        Task<IEnumerable<ItineranciaCodigoAlunoDto>> ObterCodigoAlunosPorItineranciasIds(long[] itineranciasIds);
        Task<Itinerancia> ObterEntidadeCompleta(long id);
        Task<PaginacaoResultadoDto<ItineranciaRetornoQueryDto>> ObterItineranciasPaginado(long dreId, long ueId, long turmaId, string alunoCodigo, int? situacao, int anoLetivo, DateTime? dataInicio, DateTime? dataFim, string criadoRf, Paginacao paginacao);
        Task<IEnumerable<int>> ObterAnosLetivosItinerancia();
    }
}
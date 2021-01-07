using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ListarOcorrenciasQuery : IRequest<PaginacaoResultadoDto<OcorrenciaListagemDto>>
    {
        public ListarOcorrenciasQuery(DateTime? dataOcorrenciaInicio, DateTime? dataOcorrenciaFim, string alunoNome, string titulo, long turmaId)
        {
            DataOcorrenciaInicio = dataOcorrenciaInicio;
            DataOcorrenciaFim = dataOcorrenciaFim;
            AlunoNome = alunoNome;
            Titulo = titulo;
            TurmaId = turmaId;
        }

        public DateTime? DataOcorrenciaInicio { get; set; }
        public DateTime? DataOcorrenciaFim { get; set; }
        public long TurmaId { get; set; }
        public string AlunoNome { get; set; }
        public string Titulo { get; set; }
    }
}

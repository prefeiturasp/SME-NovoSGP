using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ListarOcorrenciasQuery : IRequest<IEnumerable<OcorrenciaListagemDto>>
    {
        public ListarOcorrenciasQuery(DateTime? dataOcorrenciaInicio, DateTime? dataOcorrenciaFim, string alunoNome, string titulo)
        {
            DataOcorrenciaInicio = dataOcorrenciaInicio;
            DataOcorrenciaFim = dataOcorrenciaFim;
            AlunoNome = alunoNome;
            Titulo = titulo;
        }

        public DateTime? DataOcorrenciaInicio { get; set; }
        public DateTime? DataOcorrenciaFim { get; set; }
        public string AlunoNome { get; set; }
        public string Titulo { get; set; }
    }
}

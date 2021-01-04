using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class InserirOcorrenciaCommand : IRequest<AuditoriaDto>
    {
        public DateTime DataOcorrencia { get; set; }
        public string HoraOcorrencia { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public long OcorrenciaTipoId { get; set; }
        public IEnumerable<long> CodigosAlunos { get; set; }
        public InserirOcorrenciaCommand()
        {
            CodigosAlunos = new List<long>();
        }

        public InserirOcorrenciaCommand(DateTime dataOcorrencia, string horaOcorrencia, 
                                        string titulo, string descricao, long ocorrenciaTipoId, 
                                        IEnumerable<long> codigosAlunos)
        {
            DataOcorrencia = dataOcorrencia;
            HoraOcorrencia = horaOcorrencia;
            Titulo = titulo;
            Descricao = descricao;
            OcorrenciaTipoId = ocorrenciaTipoId;
            CodigosAlunos = codigosAlunos;
        }
    }
}

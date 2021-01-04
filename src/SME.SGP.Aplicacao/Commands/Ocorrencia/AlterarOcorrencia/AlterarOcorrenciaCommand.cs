using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class AlterarOcorrenciaCommand : IRequest<AuditoriaDto>
    {
        public long Id { get; set; }
        public DateTime DataOcorrencia { get; set; }
        public string HoraOcorrencia { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public long OcorrenciaTipoId { get; set; }
        public IEnumerable<long> CodigosAlunos { get; set; }
        public AlterarOcorrenciaCommand()
        {
            CodigosAlunos = new List<long>();
        }

        public AlterarOcorrenciaCommand(long id, DateTime dataOcorrencia, string horaOcorrencia, 
                                        string titulo, string descricao, long ocorrenciaTipoId, 
                                        IEnumerable<long> codigosAlunos)
        {
            Id = id;
            DataOcorrencia = dataOcorrencia;
            HoraOcorrencia = horaOcorrencia;
            Titulo = titulo;
            Descricao = descricao;
            OcorrenciaTipoId = ocorrenciaTipoId;
            CodigosAlunos = codigosAlunos;
        }
    }
}

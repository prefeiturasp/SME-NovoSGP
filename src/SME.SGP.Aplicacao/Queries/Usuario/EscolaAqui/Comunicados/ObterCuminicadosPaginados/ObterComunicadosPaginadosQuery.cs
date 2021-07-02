using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dto;
using SME.SGP.Infra;
using System;

namespace SME.SGP.Aplicacao
{
    public class ObterComunicadosPaginadosQuery : IRequest<PaginacaoResultadoDto<ComunicadoDto>>
    {
        public DateTime? DataEnvio { get; set; }
        public DateTime? DataExpiracao { get; set; }
        public int[] GruposId { get; set; }
        public string Titulo { get; set; }
        public int AnoLetivo { get; set; }
        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }
        public int[] Modalidades { get; set; }
        public int Semestre { get; set; }
        public string[] Turmas { get; set; }
        public long EventoId { get; set; }

        public ObterComunicadosPaginadosQuery(DateTime? dataEnvio, DateTime? dataExpiracao, string titulo, int anoLetivo, string codigoDre, string codigoUe, int[] modalidades, int semestre, string[] turmas, long eventoId)
        {
            DataEnvio = dataEnvio;
            DataExpiracao = dataExpiracao;
          //  GruposId = gruposId;
            Titulo = titulo;
            AnoLetivo = anoLetivo;
            CodigoDre = codigoDre;
            CodigoUe = codigoUe;
            Modalidades = modalidades;
            Semestre = semestre;
            Turmas = turmas;
            EventoId = eventoId;
        }
    }
}

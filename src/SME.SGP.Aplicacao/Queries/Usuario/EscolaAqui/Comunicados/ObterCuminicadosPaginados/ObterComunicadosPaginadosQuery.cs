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
        public Modalidade Modalidade { get; set; }
        public int Semestre { get; set; }
        public string[] Turmas { get; set; }

        public ObterComunicadosPaginadosQuery(DateTime? dataEnvio, DateTime? dataExpiracao, int[] gruposId, string titulo, int anoLetivo, string codigoDre, string codigoUe, Modalidade modalidade, int semestre, string[] turmas)
        {
            DataEnvio = dataEnvio;
            DataExpiracao = dataExpiracao;
            GruposId = gruposId;
            Titulo = titulo;
            AnoLetivo = anoLetivo;
            CodigoDre = codigoDre;
            CodigoUe = codigoUe;
            Modalidade = modalidade;
            Semestre = semestre;
            Turmas = turmas;
        }
    }
}

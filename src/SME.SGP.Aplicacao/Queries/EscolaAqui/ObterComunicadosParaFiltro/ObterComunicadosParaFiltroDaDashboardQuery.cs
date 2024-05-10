using MediatR;
using SME.SGP.Infra.Dtos.EscolaAqui.ComunicadosFiltro;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterComunicadosParaFiltroDaDashboardQuery : IRequest<IEnumerable<ComunicadoParaFiltroDaDashboardDto>>
    {
        public ObterComunicadosParaFiltroDaDashboardQuery(ObterComunicadosParaFiltroDaDashboardDto comunicadosFiltroDto)
        {
            AnoLetivo = comunicadosFiltroDto.AnoLetivo;
            CodigoDre = comunicadosFiltroDto.CodigoDre;
            CodigoUe = comunicadosFiltroDto.CodigoUe;
            Modalidades = comunicadosFiltroDto.Modalidades;
            Semestre = comunicadosFiltroDto.Semestre;
            AnoEscolar = comunicadosFiltroDto.AnoEscolar;
            CodigoTurma = comunicadosFiltroDto.CodigoTurma;
            DataEnvioInicial = comunicadosFiltroDto.DataEnvioInicial;
            DataEnvioFinal = comunicadosFiltroDto.DataEnvioFinal;
            Descricao = comunicadosFiltroDto.Descricao;
        }

        public int AnoLetivo { get; set; }
        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }
        public int[] Modalidades { get; set; }
        public short? Semestre { get; set; }
        public string AnoEscolar { get; set; }
        public string CodigoTurma { get; set; }
        public DateTime? DataEnvioInicial { get; set; }
        public DateTime? DataEnvioFinal { get; set; }
        public string Descricao { get; set; }

        
    }
}
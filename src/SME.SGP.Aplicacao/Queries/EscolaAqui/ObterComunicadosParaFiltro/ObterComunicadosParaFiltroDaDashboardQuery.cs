using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra.Dtos.EscolaAqui.ComunicadosFiltro;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries.EscolaAqui.ObterComunicadosParaFiltro
{
    public class ObterComunicadosParaFiltroDaDashboardQuery : IRequest<IEnumerable<ComunicadoParaFiltroDaDashboardDto>>
    {
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

        public ObterComunicadosParaFiltroDaDashboardQuery(int anoLetivo, string codigoDre, string codigoUe, int[] modalidades, short? semestre,
            string anoEscolar, string codigoTurma, DateTime? dataEnvioInicial, DateTime? dataEnvioFinal, string descricao)
        {
            AnoLetivo = anoLetivo;
            CodigoDre = codigoDre;
            CodigoUe = codigoUe;           
            Modalidades = modalidades;
            Semestre = semestre;
            AnoEscolar = anoEscolar;
            CodigoTurma = codigoTurma;
            DataEnvioInicial = dataEnvioInicial;
            DataEnvioFinal = dataEnvioFinal;
            Descricao = descricao;
        }
    }
}
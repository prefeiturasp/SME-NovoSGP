using FluentValidation;
using MediatR;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dto;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries
{ 
    public class ListarComunicadosPaginadosQuery : IRequest<PaginacaoResultadoDto<ComunicadoAlunoReduzidoDto>>
    {
        public ListarComunicadosPaginadosQuery(string dreCodigo,string ueCodigo,string turmaCodigo,string alunoCodigo)
        {
            DRECodigo = dreCodigo;
            UECodigo = ueCodigo;
            TurmaCodigo = turmaCodigo;
            AlunoCodigo = alunoCodigo;
        }

        public string DRECodigo { get; set; }
        public string UECodigo { get; set; }
        public string TurmaCodigo { get; set; }
        public string AlunoCodigo { get; set; }
    }
}

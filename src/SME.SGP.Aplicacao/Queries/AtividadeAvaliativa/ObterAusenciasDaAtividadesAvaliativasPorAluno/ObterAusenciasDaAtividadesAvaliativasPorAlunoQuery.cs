using System;
using System.Collections.Generic;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterAusenciasDaAtividadesAvaliativasPorAlunoQuery : IRequest<IEnumerable<AusenciaAlunoDto>>
    {
        public ObterAusenciasDaAtividadesAvaliativasPorAlunoQuery(string turmaCodigo, DateTime[] atividadesAvaliativasData, string componenteCurricularCodigo, string codigoAluno)
        {
            TurmaCodigo = turmaCodigo;
            AtividadesAvaliativasData = atividadesAvaliativasData;
            ComponenteCurricularCodigo = componenteCurricularCodigo;
            CodigoAluno = codigoAluno;
        }

        public string TurmaCodigo { get; set; }
        public DateTime[] AtividadesAvaliativasData { get; set; }
        public string ComponenteCurricularCodigo { get; set; }
        public string CodigoAluno { get; set; }
    }
}
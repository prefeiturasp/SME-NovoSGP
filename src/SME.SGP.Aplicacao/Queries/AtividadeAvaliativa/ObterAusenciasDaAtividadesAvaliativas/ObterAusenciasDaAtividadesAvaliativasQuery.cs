using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterAusenciasDaAtividadesAvaliativasQuery : IRequest<IEnumerable<AusenciaAlunoDto>>
    {
        public ObterAusenciasDaAtividadesAvaliativasQuery(string turmaCodigo, DateTime[] atividadesAvaliativasData, string componenteCurricularCodigo, string[] alunosId)
        {
            TurmaCodigo = turmaCodigo;
            AtividadesAvaliativasData = atividadesAvaliativasData;
            ComponenteCurricularCodigo = componenteCurricularCodigo;
            AlunosId = alunosId;
        }
        public string TurmaCodigo { get; set; }
        public DateTime[] AtividadesAvaliativasData { get; set; }
        public string ComponenteCurricularCodigo { get; set; }
        public string[] AlunosId { get; set; }

    }
}

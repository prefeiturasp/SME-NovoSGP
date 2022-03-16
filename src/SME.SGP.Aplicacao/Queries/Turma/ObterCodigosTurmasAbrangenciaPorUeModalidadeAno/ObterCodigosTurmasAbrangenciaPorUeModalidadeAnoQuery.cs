using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    class ObterCodigosTurmasAbrangenciaPorUeModalidadeAnoQuery : IRequest<IEnumerable<long>>
    {
        public ObterCodigosTurmasAbrangenciaPorUeModalidadeAnoQuery(string ueCodigo, Modalidade modalidade, int periodo, bool consideraHistorico, int anoLetivo, int[] tipos, bool desconsideraNovosAnosInfantil)
        {
            UeCodigo = ueCodigo;
            Modalidade = modalidade;
            Periodo = periodo;
            ConsideraHistorico = consideraHistorico;
            AnoLetivo = anoLetivo;
            Tipos = tipos;
            DesconsideraNovosAnosInfantil = desconsideraNovosAnosInfantil;
        }

        public string UeCodigo { get; set; }
        public Modalidade Modalidade { get; set; }
        public int Periodo { get; set; }
        public bool ConsideraHistorico { get; set; }
        public int AnoLetivo { get; set; }
        public int[] Tipos { get; set; }
        public bool DesconsideraNovosAnosInfantil { get; set; }
    }
}

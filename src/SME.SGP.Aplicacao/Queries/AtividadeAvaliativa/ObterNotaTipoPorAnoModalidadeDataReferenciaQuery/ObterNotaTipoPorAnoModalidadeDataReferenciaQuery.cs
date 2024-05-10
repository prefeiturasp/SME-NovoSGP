using MediatR;
using SME.SGP.Dominio;
using System;

namespace SME.SGP.Aplicacao
{
    public class ObterNotaTipoPorAnoModalidadeDataReferenciaQuery : IRequest<NotaTipoValor>
    {
        public ObterNotaTipoPorAnoModalidadeDataReferenciaQuery(string ano, Modalidade modalidade, DateTime dataReferencia)
        {
            Ano = ano;
            Modalidade = modalidade;
            DataReferencia = dataReferencia;
        }

        public string Ano { get; set; }
        public Modalidade Modalidade { get; set; }
        public DateTime DataReferencia { get; set; }
    }
}

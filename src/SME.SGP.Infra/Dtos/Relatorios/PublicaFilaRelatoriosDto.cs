using System;

namespace SME.SGP.Infra.Dtos
{
    public class PublicaFilaRelatoriosDto
    {
        public PublicaFilaRelatoriosDto(string fila, object filtros, string endpoint, Guid codigoCorrelacao)
        {
            Fila = fila;
            Filtros = filtros;
            Endpoint = endpoint;
            CodigoCorrelacao = codigoCorrelacao;
        }

        public string Fila { get; set; }
        public object Filtros { get; set; }
        //TODO: PENSAR EM NOME MELHOR
        public string Endpoint { get; set; }
        public Guid CodigoCorrelacao { get; set; }
    }
}

using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterAtividadesAvaliativasCalendarioProfessorPorMesDiaQuery : IRequest<IEnumerable<AtividadeAvaliativa>>
    {
        public string UeCodigo { get; set; }        
        public string DreCodigo { get; internal set; }
        public string TurmaCodigo { get; set; }
        public string CodigoRf { get; set; }
        public DateTime DataReferencia { get; set; }
    }
}

using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;

namespace SME.SGP.Aplicacao
{
    public class ObterPodeCadastrarAulaPorDataQuery : IRequest<PodeCadastrarAulaPorDataRetornoDto>
    {
        public DateTime DataAula { get; set; }
        public long TipoCalendarioId { get; set; }
        public Turma Turma { get; set; }
        public string UeCodigo { get; set; }
        public string DreCodigo { get; set; }
        public int Bimestre { get; set; }
    }
}

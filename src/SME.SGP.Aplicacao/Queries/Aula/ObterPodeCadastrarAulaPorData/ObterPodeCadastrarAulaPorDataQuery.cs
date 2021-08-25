using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;

namespace SME.SGP.Aplicacao
{
    public class ObterPodeCadastrarAulaPorDataQuery : IRequest<PodeCadastrarAulaPorDataRetornoDto>
    {
        public ObterPodeCadastrarAulaPorDataQuery() { }
        public ObterPodeCadastrarAulaPorDataQuery(DateTime dataAula, long tipoCalendarioId, Turma turma, string ueCodigo, string dreCodigo, int bimestre)
        {
            DataAula = dataAula;
            TipoCalendarioId = tipoCalendarioId;
            Turma = turma;
            UeCodigo = ueCodigo;
            DreCodigo = dreCodigo;
            Bimestre = bimestre;
        }
        public DateTime DataAula { get; set; }
        public long TipoCalendarioId { get; set; }
        public Turma Turma { get; set; }
        public string UeCodigo { get; set; }
        public string DreCodigo { get; set; }
        public int Bimestre { get; set; }
    }
}

using MediatR;
using System;

namespace SME.SGP.Aplicacao
{
    public class GerarPendenciasFechamentoCommand : IRequest<bool>
    {
        public GerarPendenciasFechamentoCommand(long componenteCurricularId,
                                                string turmaCodigo,
                                                string turmaNome,
                                                DateTime periodoEscolarInicio,
                                                DateTime periodoEscolarFim,
                                                int bimestre,
                                                long usuarioId,
                                                string usuarioRF,
                                                long fechamentoTurmaDisciplinaId,
                                                string justificativa,
                                                string criadoRF,
                                                bool componenteSemNota = false,
                                                bool registraFrequencia = true)
        {
            ComponenteCurricularId = componenteCurricularId;
            TurmaCodigo = turmaCodigo;
            TurmaNome = turmaNome;
            PeriodoEscolarInicio = periodoEscolarInicio;
            PeriodoEscolarFim = periodoEscolarFim;
            Bimestre = bimestre;
            UsuarioId = usuarioId;
            UsuarioRF = usuarioRF;
            FechamentoTurmaDisciplinaId = fechamentoTurmaDisciplinaId;
            Justificativa = justificativa;
            CriadoRF = criadoRF;
            ComponenteSemNota = componenteSemNota;
            RegistraFrequencia = registraFrequencia;
        }

        public long ComponenteCurricularId { get; }
        public string TurmaCodigo { get; }
        public string TurmaNome { get; }
        public DateTime PeriodoEscolarInicio { get; }
        public DateTime PeriodoEscolarFim { get; }
        public int Bimestre { get; }
        public long UsuarioId { get; }
        public string UsuarioRF { get; }
        public long FechamentoTurmaDisciplinaId { get; }
        public string Justificativa { get; }
        public string CriadoRF { get; }
        public bool ComponenteSemNota { get; }
        public bool RegistraFrequencia { get; }
    }
}

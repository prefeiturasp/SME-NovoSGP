﻿using System;

namespace SME.SGP.Infra
{
    public class ConsolidacaoRegistrosPedagogicosDto
    {
        public long PeriodoEscolarId { get; set; }
        public long TurmaId { get; set; }
        public string TurmaCodigo { get; set; }
        public int AnoLetivo { get; set; }
        public long ComponenteCurricularId { get; set; }
        public string ComponenteCurricular { get; set; }
        public int QuantidadeAulas { get; set; }
        public int FrequenciasPendentes { get; set; }
        public DateTime? DataUltimaFrequencia { get; set; }
        public DateTime? DataUltimoPlanoAula { get; set; }
        public DateTime? DataUltimoDiarioBordo { get; set; }
        public int DiarioBordoPendentes { get; set; }
        public int PlanoAulaPendentes { get; set; }
        public string NomeProfessor { get; set; }
        public string RFProfessor { get; set; }
        public int ModalidadeCodigo { get; set; }
        public bool CJ { get; set; }

        public ConsolidacaoRegistrosPedagogicosDto()
        {
        }

        public ConsolidacaoRegistrosPedagogicosDto(long periodoEscolarId, long turmaId, string turmaCodigo,
            int anoLetivo, long componenteCurricularId, string componenteCurricular, int quantidadeAulas, int frequenciasPendentes,
            DateTime? dataUltimaFrequencia, DateTime? dataUltimoPlanoAula, DateTime? dataUltimoDiarioBordo,
            int diarioBordoPendentes, int planoAulaPendentes, string nomeProfessor, string rFProfessor, int modalidadeCodigo)
        {
            PeriodoEscolarId = periodoEscolarId;
            TurmaId = turmaId;
            TurmaCodigo = turmaCodigo;
            AnoLetivo = anoLetivo;
            ComponenteCurricularId = componenteCurricularId;
            ComponenteCurricular = componenteCurricular;
            QuantidadeAulas = quantidadeAulas;
            FrequenciasPendentes = frequenciasPendentes;
            DataUltimaFrequencia = dataUltimaFrequencia;
            DataUltimoPlanoAula = dataUltimoPlanoAula;
            DataUltimoDiarioBordo = dataUltimoDiarioBordo;
            DiarioBordoPendentes = diarioBordoPendentes;
            PlanoAulaPendentes = planoAulaPendentes;
            NomeProfessor = nomeProfessor;
            RFProfessor = rFProfessor;
            ModalidadeCodigo = modalidadeCodigo;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Dominio
{
    public class Aula : EntidadeBase, ICloneable
    {
        private readonly IReadOnlyList<string> ComponentesDeAEEColaborativo;
        private readonly IReadOnlyList<string> ComponentesDeAEEContraturno;
        private readonly IReadOnlyList<string> ComponentesDeAulaCompartilhada;
        private readonly IReadOnlyList<string> ComponentesDeRecuperacaoParalela;
        private readonly IReadOnlyList<string> ComponentesDeTecnologiaAprendizagem;

        public Aula()
        {
            Status = EntidadeStatus.Aprovado;
            ComponentesDeAulaCompartilhada = new List<string> {
                "1116",
                "1151",
                "1150",
                "1106",
                "1122",
                "1123"
            };
            ComponentesDeRecuperacaoParalela = new List<string> {
                "1033",
                "1051",
                "1052",
                "1053",
                "1054"
            };
            ComponentesDeTecnologiaAprendizagem = new List<string> {
                "1060",
                "1061"
            };

            ComponentesDeAEEColaborativo = new List<string> {
                "1103",
            };
            ComponentesDeAEEContraturno = new List<string> {
                "1030",
            };
        }

        public bool AulaCJ { get; set; }
        public Aula AulaPai { get; set; }
        public long? AulaPaiId { get; set; }
        public DateTime DataAula { get; set; }
        public string DisciplinaId { get; set; }
        public string DisciplinaNome { get; set; }
        public string DisciplinaCompartilhadaId { get; set; }

        public bool EhAEE => ComponentesDeAEEColaborativo.Any(c => c == DisciplinaId);
        public bool EhAEEContraturno => ComponentesDeAEEContraturno.Any(c => c == DisciplinaId);
        public bool EhAulaCompartilhada => ComponentesDeAulaCompartilhada.Any(c => c == DisciplinaId);
        public bool EhRecuperacaoParalela => ComponentesDeRecuperacaoParalela.Any(c => c == DisciplinaId);
        public bool EhTecnologiaAprendizagem => ComponentesDeTecnologiaAprendizagem.Any(c => c == DisciplinaId);

        public bool Excluido { get; set; }
        public bool Migrado { get; set; }
        public bool PermiteSubstituicaoFrequencia => !(EhAEE || EhAEEContraturno || EhRecuperacaoParalela);

        public string ProfessorRf { get; set; }

        public int Quantidade { get; set; }

        public RecorrenciaAula RecorrenciaAula { get; set; }

        public EntidadeStatus Status { get; set; }

        public TipoAula TipoAula { get; set; }

        public TipoCalendario TipoCalendario { get; set; }

        public long TipoCalendarioId { get; set; }

        public string TurmaId { get; set; }

        public string UeId { get; set; }

        public long? WorkflowAprovacaoId { get; set; }

        public void AdicionarAulaPai(Aula aula)
        {
            AulaPai = aula ?? throw new NegocioException("É necessário informar uma aula.");
            AulaPaiId = aula.Id;
        }

        public void AprovaWorkflow()
        {
            if (Status != EntidadeStatus.AguardandoAprovacao)
                throw new NegocioException("Esta aula não pode ser aprovada.");

            Status = EntidadeStatus.Aprovado;
        }

        public object Clone()
        {
            return new Aula
            {
                AlteradoEm = AlteradoEm,
                AlteradoPor = AlteradoPor,
                AlteradoRF = AlteradoRF,
                CriadoEm = CriadoEm,
                CriadoPor = CriadoPor,
                CriadoRF = CriadoRF,
                Excluido = Excluido,
                UeId = UeId,
                AulaPai = AulaPai,
                DisciplinaId = DisciplinaId,
                DisciplinaNome = DisciplinaNome,
                AulaPaiId = AulaPaiId,
                DataAula = DataAula,
                Migrado = Migrado,
                TipoCalendario = TipoCalendario,
                ProfessorRf = ProfessorRf,
                Quantidade = Quantidade,
                RecorrenciaAula = RecorrenciaAula,
                TipoAula = TipoAula,
                TipoCalendarioId = TipoCalendarioId,
                TurmaId = TurmaId,
                AulaCJ = AulaCJ
            };
        }

        public void EnviarParaWorkflowDeAprovacao(long idWorkflow)
        {
            WorkflowAprovacaoId = idWorkflow;
            Status = EntidadeStatus.AguardandoAprovacao;
        }

        public bool PermiteRegistroFrequencia(Turma turma)
        {
            if (turma == null)
                throw new NegocioException("A turma deve ser informada.");

            return !(EhAulaCompartilhada || (EhTecnologiaAprendizagem && turma.ModalidadeCodigo == Modalidade.EJA));
        }

        public void PodeSerAlterada(Usuario usuario)
        {
            if (AulaCJ)
            {
                if (usuario.EhProfessor() || usuario.EhProfessorCj())
                    if (usuario.CodigoRf != this.CriadoRF)
                        throw new NegocioException("Você não pode alterar esta Atividade Avaliativa.");
            }
        }

        public void ReprovarWorkflow()
        {
            if (Status != EntidadeStatus.AguardandoAprovacao)
                throw new NegocioException("Este Evento não pode ser recusado.");

            Status = EntidadeStatus.Recusado;
        }
    }
}
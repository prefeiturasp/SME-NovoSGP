namespace SME.SGP.Dominio
{
    public class ConselhoClasseAluno: EntidadeBase
    {        
        public long ConselhoClasseId { get; set; }
        public ConselhoClasse ConselhoClasse { get; set; }
        public string AlunoCodigo { get; set; }
        public string RecomendacoesAluno { get; set; }
        public string RecomendacoesFamilia { get; set; }
        public string AnotacoesPedagogicas { get; set; }

        public long? ConselhoClasseParecerId { get; set; }
        public ConselhoClasseParecerConclusivo ConselhoClasseParecer { get; set; }

        public bool Excluido { get; set; }
        public bool Migrado { get; set; }      
        public bool ParecerAlteradoManual { get; set; }
    }
}

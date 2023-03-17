namespace SME.SGP.Infra
{
    public class FrequenciaAlunoAulaTurmaDto
    {
        public FrequenciaAlunoAulaTurmaDto(string codigoAluno, int valor, long aulaId, long registroFrequenciaId, long turmaId)
        {
            CodigoAluno = codigoAluno;
            Valor = valor;
            AulaId = aulaId;
            RegistroFrequenciaId = registroFrequenciaId;
            TurmaId = turmaId;
        }
        public string CodigoAluno { get; set; }
        public int Valor { get; set; }
        public long AulaId { get; set; }
        public long RegistroFrequenciaId { get; set; }
        public long TurmaId { get; set; }


        public string ObterInformacoes()
        {
            return
                $"CodigoAluno: {CodigoAluno} | Valor: {Valor} | AulaId: {AulaId} | RegistroFrequenciaId: {RegistroFrequenciaId} | TurmaId: {TurmaId}";
        }
    }
}
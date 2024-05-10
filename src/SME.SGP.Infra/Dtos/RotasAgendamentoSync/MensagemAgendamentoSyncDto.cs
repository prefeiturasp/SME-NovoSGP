namespace SME.SGP.Infra
{
    public class MensagemAgendamentoSyncDto
    {
        public MensagemAgendamentoSyncDto(string rota, object objeto)
        {
            Rota = rota;
            Objeto = objeto;
        }

        public string Rota { get; }
        public object Objeto { get; }
    }
}

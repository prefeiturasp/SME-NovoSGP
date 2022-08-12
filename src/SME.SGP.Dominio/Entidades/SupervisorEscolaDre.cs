namespace SME.SGP.Dominio
{
    public class SupervisorEscolaDre : EntidadeBase
    {
        public string DreId { get; set; }
        public string EscolaId { get; set; }
        public string SupervisorId { get; set; }
        public bool Excluido { get; set; }

        public void Excluir()
        {
            if (Excluido)
                throw new NegocioException("Este supervisor já está excluido.");
            Excluido = true;
        }
    }
}
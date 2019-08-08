namespace SME.SGP.Dominio
{
    public class PlanoCiclo : EntidadeBase
    {
        public PlanoCiclo()
        {
            //Matrizes = new HashSet<MatrizSaberPlano>();
        }

        public string Descricao { get; set; }
        //public ICollection<MatrizSaberPlano> Matrizes { get; set; }
    }
}
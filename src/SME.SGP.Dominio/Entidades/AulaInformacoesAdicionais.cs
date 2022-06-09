namespace SME.SGP.Dominio
{
    public class AulaInformacoesAdicionais : Aula
    {
        public bool PossuiFrequencia { get; set; }
        public bool RegistroFerquenciaExcluido { get; set; }
        public bool PossuiPlanoAula { get; set; }
        public bool RegistroPlanoAulaExcluido { get; set; }

    }
}

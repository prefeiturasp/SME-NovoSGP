using SME.SGP.Dominio;

namespace SME.SGP.Infra.Utilitarios
{
    public static class AuditoriaUtil
    {
        public static string MontarTextoAuditoriaAlteracao(EntidadeBase fechamentoTurmaDisciplina, bool ehNota)
        {
            if (fechamentoTurmaDisciplina.NaoEhNulo() && !string.IsNullOrEmpty(fechamentoTurmaDisciplina.AlteradoPor))
            {
                return
                    $"{(ehNota ? "Notas" : "Conceitos")} finais {(ehNota ? "alteradas" : "alterados")} por {fechamentoTurmaDisciplina.AlteradoPor}({fechamentoTurmaDisciplina.AlteradoRF}) em {fechamentoTurmaDisciplina.AlteradoEm.GetValueOrDefault():dd/MM/yyyy}, às {fechamentoTurmaDisciplina.AlteradoEm.GetValueOrDefault():HH:mm}.";
            }

            return string.Empty;
        }        
        
        public static string MontarTextoAuditoriaInclusao(EntidadeBase fechamentoTurmaDisciplina, bool ehNota)
        {
            var criadorRf =
                fechamentoTurmaDisciplina.NaoEhNulo() && fechamentoTurmaDisciplina.CriadoRF != "0" &&
                !string.IsNullOrEmpty(fechamentoTurmaDisciplina.CriadoRF)
                    ? $"({fechamentoTurmaDisciplina.CriadoRF})"
                    : "";

            return fechamentoTurmaDisciplina.NaoEhNulo()
                ? $"{(ehNota ? "Notas" : "Conceitos")} finais {(ehNota ? "incluídas" : "incluídos")} por {fechamentoTurmaDisciplina.CriadoPor}{criadorRf} em {fechamentoTurmaDisciplina.CriadoEm:dd/MM/yyyy}, às {fechamentoTurmaDisciplina.CriadoEm:HH:mm}."
                : string.Empty;
        }        
    }
}
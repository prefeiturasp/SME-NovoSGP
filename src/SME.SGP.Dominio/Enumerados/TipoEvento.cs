using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dominio
{
    public enum TipoEvento
    {
        [Display(Name = "Conselho de Classe")]
        ConselhoDeClasse = 1,

        [Display(Name = "Evento DRE")]
        EventoDRE = 2,

        [Display(Name = "Fechamento de Bimestre")]
        FechamentoBimestre = 3,

        [Display(Name = "Feriado")]
        Feriado = 4,

        [Display(Name = "Férias docentes")]
        FeriasDocentes = 5,

        [Display(Name = "Liberação Excepcional")]
        LiberacaoExcepcional = 6,

        [Display(Name = "Liberação do Boletim")]
        LiberacaoBoletim = 7,

        [Display(Name = "Organização Escolar")]
        OrganizacaoEscolar = 8,

        [Display(Name = "Outros")]
        Outros = 9,

        [Display(Name = "Projeto Político Pedagógico")]
        ProjetoPoliticoPedagogico = 10,

        [Display(Name = "Recesso")]
        Recesso = 11,

        [Display(Name = "Recreio nas Férias")]
        RecreioNasFerias = 12,

        [Display(Name = "Reposição de Aula")]
        ReposicaoDeAula = 13,

        [Display(Name = "Reposição do Dia")]
        ReposicaoDoDia = 14,

        [Display(Name = "Reposição no recesso")]
        ReposicaoNoRecesso = 15,

        [Display(Name = "Reunião Pedagógica")]
        ReuniaoPedagogica = 16,

        [Display(Name = "Reunião de APM")]
        ReuniaoAPM = 17,

        [Display(Name = "Reunião de Conselho de Escola")]
        ReuniaoConselhoEscola = 18,

        [Display(Name = "Reunião de Responsáveis")]
        ReuniaoResponsaveis = 19,

        [Display(Name = "Sondagem")]
        Sondagem = 20,

        [Display(Name = "Suspensão de Atividades")]
        SuspensaoAtividades = 21,

        [Display(Name = "Formação")]
        Formacao = 22,

        [Display(Name = "Curso")]
        Curso = 23,

        [Display(Name = "Encontro Mensal")]
        EncontroMensal = 24,

        [Display(Name = "Seminário")]
        Seminario = 25,

        [Display(Name = "Palestra")]
        Palestra = 26,

        [Display(Name = "Reunião")]
        Reuniao = 27,

        [Display(Name = "Itinerância PAAI")]
        ItineranciaPAAI = 28,
    }
}
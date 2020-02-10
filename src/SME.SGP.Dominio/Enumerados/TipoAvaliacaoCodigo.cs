using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SME.SGP.Dominio
{
    public enum TipoAvaliacaoCodigo
    {
        [Display(Name = "Avaliação bimestral")]
        AvaliacaoBimestral = 1,

        [Display(Name = "Avaliação mensal")]
        AvaliacaoMensal = 2,

        [Display(Name = "Chamada oral")]
        ChamadaOral = 3,

        [Display(Name = "Debate")]
        Debate = 4,

        [Display(Name = "Dramatização")]
        Dramatizacao = 5,

        [Display(Name = "Estudo de meio")]
        EstudoDeMeio = 6,

        [Display(Name = "Pesquisa")]
        Pesquisa = 7,

        [Display(Name = "Produção de texto")]
        ProducaoDeTexto = 8,

        [Display(Name = "Projeto escolar")]
        ProjetoEscolar = 9,

        [Display(Name = "Seminário")]
        Seminario = 10,

        [Display(Name = "TCA")]
        TCA = 11,

        [Display(Name = "Teste")]
        Teste = 12,

        [Display(Name = "Teste de múltipla escolha")]
        TesteMultiplaEscolha = 13,

        [Display(Name = "Texto")]
        Texto = 14,

        [Display(Name = "Trabalho individual")]
        TrabalhoIndividual = 15,

        [Display(Name = "Trabalho em grupo")]
        TrabalhoEmGrupo = 16
    }
}

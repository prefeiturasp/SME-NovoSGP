using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class RelatorioSemestralAlunoSecaoDto
    {
        public RelatorioSemestralAlunoSecaoDto() { }
        public RelatorioSemestralAlunoSecaoDto(long id, string nome, string descricao, bool obrigatorio, string valor)
        {
            Id = id;
            Nome = nome;
            Descricao = descricao;
            Obrigatorio = obrigatorio;
            Valor = valor;
        }

        public long Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public bool Obrigatorio { get; set; }
        public string Valor { get; set; }
        public int Ordem { get; set; }
    }
    public class RelatorioSemestralAlunoSecaoResumidaDto
    {
        public RelatorioSemestralAlunoSecaoResumidaDto() { }
        public string SecaoAtual { get; set; }
        public string SecaoNovo { get; set; }
    }
}

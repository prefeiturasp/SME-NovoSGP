using SME.SGP.Dominio.Interfaces;
using System;
using System.Linq;

namespace SME.SGP.Dominio.Servicos
{
    public class ServicoPendenciaFechamento : IServicoPendenciaFechamento
    {
        private readonly IRepositorioAtividadeAvaliativa repositorioAtividadeAvaliativa;
        private readonly IRepositorioPendencia repositorioPendencia;

        public ServicoPendenciaFechamento(IRepositorioAtividadeAvaliativa repositorioAtividadeAvaliativa,
                                          IRepositorioPendencia repositorioPendencia)
        {
            this.repositorioAtividadeAvaliativa = repositorioAtividadeAvaliativa ?? throw new ArgumentNullException(nameof(repositorioAtividadeAvaliativa));
            this.repositorioPendencia = repositorioPendencia ?? throw new ArgumentNullException(nameof(repositorioPendencia));
        }

        public void ValidarAvaliacoesSemNotas(long fechamentoId, string codigoTurma, string disciplinaId, DateTime inicioPeriodo, DateTime fimPeriodo)
        {
            var avaliacoesSemNotaParaNenhumAluno = repositorioAtividadeAvaliativa.ObterAtividadesAvaliativasSemNotaParaNenhumAluno(codigoTurma,
                                                                            disciplinaId,
                                                                            inicioPeriodo,
                                                                            fimPeriodo);

            if (avaliacoesSemNotaParaNenhumAluno != null && avaliacoesSemNotaParaNenhumAluno.Any())
            {
                foreach (var avaliacao in avaliacoesSemNotaParaNenhumAluno)
                {
                    var pendencia = new Pendencia("Avaliação sem notas/conceitos lançados",
                                                  $"A avaliação '{avaliacao.NomeAvaliacao}' não teve notas lançadas para nenhum aluno",
                                                  fechamentoId);

                    repositorioPendencia.Salvar(pendencia);
                }
            }
        }
    }
}
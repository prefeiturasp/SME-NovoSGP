using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultasNotasConceitos : IConsultasNotasConceitos
    {
        private readonly IConsultaAtividadeAvaliativa consultasAtividadeAvaliativa;
        private readonly IServicoEOL servicoEOL;

        public ConsultasNotasConceitos(IServicoEOL servicoEOL, IConsultaAtividadeAvaliativa consultasAtividadeAvaliativa)
        {
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
            this.consultasAtividadeAvaliativa = consultasAtividadeAvaliativa ?? throw new ArgumentNullException(nameof(consultasAtividadeAvaliativa));
        }

        public async Task<NotasConceitosRetornoDto> ListarNotasConceitos(string turmaCodigo, int? bimestre, int anoLetivo, string disciplinaCodigo, Modalidade modalidade)
        {
            ModalidadeTipoCalendario modalidadeTipoCalendario = (modalidade == Modalidade.Fundamental || modalidade == Modalidade.Medio) ? ModalidadeTipoCalendario.FundamentalMedio : ModalidadeTipoCalendario.EJA;

            var atividadesAvaliativaEBimestres = await consultasAtividadeAvaliativa.ObterAvaliacoesEBimestres(turmaCodigo, disciplinaCodigo, anoLetivo, bimestre, modalidadeTipoCalendario);

            if (atividadesAvaliativaEBimestres.Item1 == null || !atividadesAvaliativaEBimestres.Item1.Any())
                throw new NegocioException("Não foi possível localizar atividades avaliativas");

            var alunos = await servicoEOL.ObterAlunosPorTurma(turmaCodigo);

            if (alunos == null || !alunos.Any())
                throw new NegocioException("Não foi encontrado alunos para a turma informada");

            var retorno = new NotasConceitosRetornoDto();

            for (int i = 0; i < atividadesAvaliativaEBimestres.quantidadeBimestres; i++)
            {
                var valorBimestreAtual = i + 1;
                var bimestreParaAdicionar = new NotasConceitosBimestreRetornoDto() { Descricao = $"{valorBimestreAtual}º Bimestre", Numero = valorBimestreAtual };

                if (valorBimestreAtual == atividadesAvaliativaEBimestres.bimestreAtual)
                {
                    var listaAlunosDoBimestre = new List<NotasConceitosAlunoRetornoDto>();

                    var atividadesAvaliativasdoBimestre = atividadesAvaliativaEBimestres.Item1.Where(a => a.DataAvaliacao.Date >= atividadesAvaliativaEBimestres.periodoAtual.PeriodoInicio.Date
                        && atividadesAvaliativaEBimestres.periodoAtual.PeriodoFim.Date >= a.DataAvaliacao.Date)
                        .OrderBy(a => a.DataAvaliacao)
                        .ToList();

                    foreach (var aluno in alunos)
                    {
                        var notaConceitoAluno = new NotasConceitosAlunoRetornoDto() { Id = aluno.CodigoAluno, Nome = aluno.NomeSocialAluno, NumeroChamada = aluno.NumeroAlunoChamada };
                        var notasAvaliacoes = new List<NotasConceitosNotaAvaliacaoRetornoDto>();

                        foreach (var atividadeAvaliativa in atividadesAvaliativasdoBimestre)
                        {
                            //TODO: Buscar a Nota se já foi lançada
                            //TODO: Buscar se houve ausencia
                            //TODO: Buscar se pode editar

                            var notaAvaliacao = new NotasConceitosNotaAvaliacaoRetornoDto() { AtividadeAvaliativaId = atividadeAvaliativa.Id };

                            notasAvaliacoes.Add(notaAvaliacao);
                        }

                        notaConceitoAluno.NotasAvaliacoes = notasAvaliacoes;
                        listaAlunosDoBimestre.Add(notaConceitoAluno);
                    }

                    foreach (var avaliacao in atividadesAvaliativasdoBimestre)
                    {
                        var avaliacaoDoBimestre = new NotasConceitosAvaliacaoRetornoDto() { Id = avaliacao.Id, Data = avaliacao.DataAvaliacao, Descricao = avaliacao.DescricaoAvaliacao, Nome = avaliacao.NomeAvaliacao };
                        bimestreParaAdicionar.Avaliacoes.Add(avaliacaoDoBimestre);
                    }

                    bimestreParaAdicionar.Alunos = listaAlunosDoBimestre;
                }

                retorno.Bimestres.Add(bimestreParaAdicionar);
            }

            return retorno;
        }
    }
}
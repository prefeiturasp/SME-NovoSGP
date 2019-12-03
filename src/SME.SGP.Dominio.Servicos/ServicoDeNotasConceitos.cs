using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Servicos
{
    public class ServicoDeNotasConceitos
    {
        private readonly IRepositorioAtividadeAvaliativa repositorioAtividadeAvaliativa;
        private readonly IServicoEOL servicoEOL;
        private readonly IConsultasAbrangencia consultasAbrangencia;
        private readonly IRepositorioNotaTipoValor repositorioNotaTipoValor;
        private readonly IRepositorioCiclo repositorioCiclo;

        public ServicoDeNotasConceitos(IRepositorioAtividadeAvaliativa repositorioAtividadeAvaliativa,
            IServicoEOL servicoEOL, IConsultasAbrangencia consultasAbrangencia,
            IRepositorioNotaTipoValor repositorioNotaTipoValor, IRepositorioCiclo repositorioCiclo)
        {
            this.repositorioAtividadeAvaliativa = repositorioAtividadeAvaliativa ?? throw new ArgumentNullException(nameof(repositorioAtividadeAvaliativa));
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
            this.consultasAbrangencia = consultasAbrangencia ?? throw new ArgumentNullException(nameof(consultasAbrangencia));
            this.repositorioNotaTipoValor = repositorioNotaTipoValor ?? throw new ArgumentNullException(nameof(repositorioNotaTipoValor));
            this.repositorioCiclo = repositorioCiclo ?? throw new ArgumentNullException(nameof(repositorioCiclo));
        }

        public async void Salvar(IEnumerable<NotasConceitos> notasConceitos, string professorRf, string turmaId)
        {
            var idsAtividadesAvaliativas = notasConceitos.Select(x => x.AtividadeAvaliativaID);

            var atividadesAvaliativas = repositorioAtividadeAvaliativa.ListarPorIds(idsAtividadesAvaliativas);

            var alunos = await servicoEOL.ObterAlunosPorTurma(turmaId);

            if (alunos == null || !alunos.Any())
                throw new NegocioException("Não foi encontrado nenhum aluno para a turma informada");

            var tiposPorAvaliacoes = TipoNotaAvaliacoes(atividadesAvaliativas);

            ValidarAvaliacoes(idsAtividadesAvaliativas, atividadesAvaliativas, professorRf);

            ValidarEntidade(notasConceitos, alunos, tiposPorAvaliacoes, null, professorRf, null);
        }

        private void ValidarEntidade(IEnumerable<NotasConceitos> notasConceitos, IEnumerable<AlunoPorTurmaResposta> alunos, Dictionary<long, TipoNota> tipoNota, IEnumerable<Conceito> conceitos, string professorRf, NotaParametro notaParametro)
        {
            notasConceitos.ToList().ForEach(notaConceito =>
            {
                notaConceito.Validar(professorRf);

                var aluno = alunos.FirstOrDefault(a => a.CodigoAluno.Equals(notaConceito.AlunoId));

                if (aluno == null)
                    throw new NegocioException($"Não foi encontrado aluno com o codigo {notaConceito.AlunoId}");

                var notaTipo = tipoNota.FirstOrDefault(x => x.Key == notaConceito.TipoNota);

                if (notaTipo.Value == TipoNota.Nota)
                    notaConceito.ValidarNota(notaParametro, aluno.NomeAluno);
                else
                    notaConceito.ValidarConceitos(conceitos, aluno.NomeAluno);
            });
        }

        private void ValidarAvaliacoes(IEnumerable<long> avaliacoesAlteradasIds, IEnumerable<AtividadeAvaliativa> atividadesAvaliativas, string professorRf)
        {
            if (atividadesAvaliativas == null || !atividadesAvaliativas.Any())
                throw new NegocioException("Não foi encontrada nenhuma das avaliações informadas");

            ValidarSeAtividadesAvaliativasExistem(avaliacoesAlteradasIds, atividadesAvaliativas);

            atividadesAvaliativas.ToList().ForEach(atividadeAvaliativa => ValidarDataAvaliacaoECriador(atividadeAvaliativa, professorRf));
        }

        private static void ValidarSeAtividadesAvaliativasExistem(IEnumerable<long> avaliacoesAlteradasIds, IEnumerable<AtividadeAvaliativa> avaliacoes)
        {
            avaliacoesAlteradasIds.ToList().ForEach(avalicaoAlteradaId =>
            {
                var atividadeavaliativa = avaliacoes.FirstOrDefault(avaliacao => avaliacao.Id == avalicaoAlteradaId);

                if (atividadeavaliativa == null)
                    throw new NegocioException($"Não foi encontrada atividade avaliativa com o codigo {avalicaoAlteradaId}");
            });
        }

        private void ValidarDataAvaliacaoECriador(AtividadeAvaliativa atividadeAvaliativa, string professorRf)
        {
            if (atividadeAvaliativa.DataAvaliacao > DateTime.Today)
                throw new NegocioException("Não é possivel atribuir notas para atividades avaliativas futuras");

            if (!atividadeAvaliativa.ProfessorRf.Equals(professorRf))
                throw new NegocioException("Somente o professor que criou a atividade avaliativa, pode atribir e ou editar notas");
        }

        private Dictionary<long, TipoNota> TipoNotaAvaliacoes(IEnumerable<AtividadeAvaliativa> atividadesAvaliativas)
        {
            var retorno = new Dictionary<long, TipoNota>();

            atividadesAvaliativas.ToList().ForEach(atividadeAvaliativa =>
            {
                var tipoValor = TipoNotaPorAvaliacao(atividadeAvaliativa);

                retorno.Add(atividadeAvaliativa.Id, tipoValor);
            });

            return retorno;
        }

        private TipoNota TipoNotaPorAvaliacao(AtividadeAvaliativa atividadeAvaliativa)
        {
            var notaTipo = ObterNotaTipo(atividadeAvaliativa.TurmaId, atividadeAvaliativa.DataAvaliacao).Result;

            if (notaTipo == null)
                throw new NegocioException("Não foi encontrado tipo de nota para a avaliação especificada");

            return notaTipo.TipoNota;
        }

        private async Task<NotaTipoValor> ObterNotaTipo(string turmaId, DateTime dataAvaliacao)
        {
            var turma = await consultasAbrangencia.ObterAbrangenciaTurma(turmaId);

            if (turma == null)
                throw new NegocioException("Não foi encontrada a turma informada");

            var ciclo = repositorioCiclo.ObterCicloPorAnoModalidade(turma.Ano, turma.Modalidade);

            if (ciclo == null)
                throw new NegocioException("Não foi encontrado o ciclo da turma informada");

            return repositorioNotaTipoValor.ObterPorCicloIdDataAvalicacao(ciclo.Id, dataAvaliacao);
        }
    }
}

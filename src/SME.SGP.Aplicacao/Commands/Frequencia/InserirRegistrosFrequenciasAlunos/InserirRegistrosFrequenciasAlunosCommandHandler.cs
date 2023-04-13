using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class InserirRegistrosFrequenciasAlunosCommandHandler : IRequestHandler<InserirRegistrosFrequenciasAlunosCommand, bool>
    {
        private readonly IRepositorioRegistroFrequenciaAluno repositorioRegistroFrequenciaAluno;
        private readonly IRepositorioFrequenciaPreDefinida repositorioFrequenciaPreDefinida;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMediator mediator;

        private const int INSERIR = 0;
        private const int ALTERAR = 1;
        private const int EXCLUIR_COMPENSACAO = 2;

        public InserirRegistrosFrequenciasAlunosCommandHandler(IRepositorioRegistroFrequenciaAluno repositorioRegistroFrequenciaAluno,
            IRepositorioFrequenciaPreDefinida repositorioFrequenciaPreDefinida,
            IUnitOfWork unitOfWork, IMediator mediator)
        {
            this.repositorioRegistroFrequenciaAluno = repositorioRegistroFrequenciaAluno ?? throw new ArgumentNullException(nameof(repositorioRegistroFrequenciaAluno));
            this.repositorioFrequenciaPreDefinida = repositorioFrequenciaPreDefinida ?? throw new ArgumentNullException(nameof(repositorioFrequenciaPreDefinida));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(InserirRegistrosFrequenciasAlunosCommand request, CancellationToken cancellationToken)
        {
            var dicionarioFrequenciaAlunoECompensacoesAusencias = await ObterDicionarioFrequenciaAlunoParaPersistirECompensacoesParaExcluir(request);
            var dicionarioPreDefinida = await ObterDicionarioFrequenciaPreDefinidaParaPersistir(request);
            
            var informacoesFrequencia = FormatarInformacoesFrequencia(dicionarioFrequenciaAlunoECompensacoesAusencias, request.RegistroFrequenciaId, request.TurmaId, request.AulaId);

            unitOfWork.IniciarTransacao();
            try
            {
                await CadastreFrequenciaAlunoAtualizarCompensacoes(dicionarioFrequenciaAlunoECompensacoesAusencias);
                await CadastreFrequenciaPreDefinida(dicionarioPreDefinida);

                unitOfWork.PersistirTransacao();
                return true;
            }
            catch (Exception ex)
            {
                unitOfWork.Rollback();
                await mediator.Send(new SalvarLogViaRabbitCommand($"Erro ao registrar a frequência do aluno e a frequência pré definida: {informacoesFrequencia}. Detalhes : {ex}", LogNivel.Critico, LogContexto.Frequencia));
                return false;
            }
        }

        private static string FormatarInformacoesFrequencia(Dictionary<int, List<RegistroFrequenciaAluno>> dicionarioFrequenciaAluno, long registroFrequenciaId, long turmaId, long aulaId)
        {
            var alunosValores = dicionarioFrequenciaAluno.SelectMany(s => s.Value.Select(a => new {a.CodigoAluno, a.Valor}).ToList()).ToList();

            var informacoesFrequencia = new { registroFrequenciaId, turmaId, aulaId, alunosValores };
            
            var json = JsonConvert.SerializeObject(informacoesFrequencia, Formatting.Indented);
            
            return json;
        }

        private async Task CadastreFrequenciaAlunoAtualizarCompensacoes(Dictionary<int, List<RegistroFrequenciaAluno>> dicionarioFrequenciaAluno)
        {
            await repositorioRegistroFrequenciaAluno.InserirVariosComLog(dicionarioFrequenciaAluno[INSERIR]);

            foreach (var frequenciaAluno in dicionarioFrequenciaAluno[ALTERAR])
                await repositorioRegistroFrequenciaAluno.SalvarAsync(frequenciaAluno);

            if (dicionarioFrequenciaAluno[EXCLUIR_COMPENSACAO].Any())
                await mediator.Send(new ExcluirCompensacaoAusenciaAlunoEAulaPorRegistroFrequenciaIdsCommand(dicionarioFrequenciaAluno[EXCLUIR_COMPENSACAO].Select(t => t.Id)));
        }

        private async Task CadastreFrequenciaPreDefinida(Dictionary<int, List<FrequenciaPreDefinida>> dicionario)
        {
            await repositorioFrequenciaPreDefinida.InserirVarios(dicionario[INSERIR]);

            foreach (var frequenciaPreDefinida in dicionario[ALTERAR])
                await repositorioFrequenciaPreDefinida.Atualizar(frequenciaPreDefinida);
        }

        private async Task<Dictionary<int, List<RegistroFrequenciaAluno>>> ObterDicionarioFrequenciaAlunoParaPersistirECompensacoesParaExcluir(InserirRegistrosFrequenciasAlunosCommand request)
        {
            var registroFrequenciasAlunos = new Dictionary<int, List<RegistroFrequenciaAluno>>
            {
                { INSERIR, new List<RegistroFrequenciaAluno>() },
                { ALTERAR, new List<RegistroFrequenciaAluno>() },
                { EXCLUIR_COMPENSACAO, new List<RegistroFrequenciaAluno>() },
            };

            var registroFrequenciaAlunoAtual = await mediator.Send(new ObterRegistroDeFrequenciaAlunoPorIdRegistroQuery(request.RegistroFrequenciaId));

            foreach (var registroFrequenciaAlunoDto in request.Frequencias)
            {
                foreach (var aulaRegistrada in registroFrequenciaAlunoDto.Aulas)
                {
                    var frequenciaAluno = registroFrequenciaAlunoAtual.FirstOrDefault(fr => fr.NumeroAula == aulaRegistrada.NumeroAula && fr.CodigoAluno == registroFrequenciaAlunoDto.CodigoAluno);
                    var valorFrequencia = ObterValorFrequencia(registroFrequenciaAlunoDto.TipoFrequenciaPreDefinido, aulaRegistrada.TipoFrequencia);

                    if (frequenciaAluno != null)
                    {
                        if (frequenciaAluno.Valor != (int)valorFrequencia)
                        {
                            if (frequenciaAluno.Valor == (int)TipoFrequencia.F && valorFrequencia != TipoFrequencia.F)
                                registroFrequenciasAlunos[EXCLUIR_COMPENSACAO].Add(frequenciaAluno);

                            frequenciaAluno.Valor = (int)valorFrequencia;
                            frequenciaAluno.AulaId = request.AulaId;
                            registroFrequenciasAlunos[ALTERAR].Add(frequenciaAluno);
                        }
                    }
                    else
                    {
                        var novafrequencia = new RegistroFrequenciaAluno()
                        {
                            CodigoAluno = registroFrequenciaAlunoDto.CodigoAluno,
                            NumeroAula = aulaRegistrada.NumeroAula,
                            Valor = (int)valorFrequencia,
                            AulaId = request.AulaId,
                            RegistroFrequenciaId = request.RegistroFrequenciaId
                        };

                        registroFrequenciasAlunos[INSERIR].Add(novafrequencia);
                    }
                }
            }

            return registroFrequenciasAlunos;
        }

        private async Task<Dictionary<int, List<FrequenciaPreDefinida>>> ObterDicionarioFrequenciaPreDefinidaParaPersistir(InserirRegistrosFrequenciasAlunosCommand request)
        {
            var dicionario = new Dictionary<int, List<FrequenciaPreDefinida>>();
            var listaDeFrequenciaDefinidaCadastrada = await mediator.Send(new ObterFrequenciasPreDefinidasPorTurmaComponenteQuery(request.TurmaId, request.ComponenteCurricularId));

            dicionario.Add(INSERIR, new List<FrequenciaPreDefinida>());
            dicionario.Add(ALTERAR, new List<FrequenciaPreDefinida>());

            foreach (var frequencia in request.Frequencias)
            {
                var frequenciaDefinida = listaDeFrequenciaDefinidaCadastrada.OrderByDescending(y => y.Id).FirstOrDefault(fr => fr.CodigoAluno == frequencia.CodigoAluno);
                var tipoFrequencia = ObtenhaValorPreDefinido(frequencia.TipoFrequenciaPreDefinido);

                if (frequenciaDefinida != null)
                {
                    frequenciaDefinida.TipoFrequencia = tipoFrequencia;
                    dicionario[ALTERAR].Add(frequenciaDefinida);
                }
                else
                {
                    var frequenciaPreDefinida = new FrequenciaPreDefinida()
                    {
                        CodigoAluno = frequencia.CodigoAluno,
                        TurmaId = request.TurmaId,
                        ComponenteCurricularId = request.ComponenteCurricularId,
                        TipoFrequencia = tipoFrequencia
                    };

                    dicionario[INSERIR].Add(frequenciaPreDefinida);
                }
            }

            return dicionario;
        }

        private TipoFrequencia ObterValorFrequencia(string tipoFrequenciaPreDefinido, string tipoFrequencia)
        {
            return ObtenhaValor(tipoFrequencia, ObtenhaValorPreDefinido(tipoFrequenciaPreDefinido));
        }

        private TipoFrequencia ObtenhaValorPreDefinido(string tipoFrequenciaPreDefinido)
        {
            return ObtenhaValor(tipoFrequenciaPreDefinido, TipoFrequencia.C);
        }

        private TipoFrequencia ObtenhaValor(string valorFrequencia, TipoFrequencia tipoFrequenciaElse)
        {
            return !string.IsNullOrEmpty(valorFrequencia) ?
                      (TipoFrequencia)Enum.Parse(typeof(TipoFrequencia), valorFrequencia) :
                      tipoFrequenciaElse;
        }
    }
}

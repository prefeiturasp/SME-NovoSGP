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
            var dicionarioFrequenciaAluno = await ObterDicionarioFrequenciaAlunoParaPersistir(request);
            var dicionarioPreDefinida = await ObterDicionarioFrequenciaPreDefinidaParaPersistir(request);
            
            var informacoesFrequencia = FormatarInformacoesFrequencia(dicionarioFrequenciaAluno, request.RegistroFrequenciaId, request.TurmaId, request.AulaId);

            using (var transacao = unitOfWork.IniciarTransacao())
            {
                try
                {
                    await CadastreFrequenciaAluno(dicionarioFrequenciaAluno);
                    await CadastreFrequenciaPreDefinida(dicionarioPreDefinida);

                    unitOfWork.PersistirTransacao();
                    return true;
                }
                catch (Exception ex)
                {
                    unitOfWork.Rollback();
                    await mediator.Send(new SalvarLogViaRabbitCommand($"Erro ao registrar a frequência do aluno e a frequência pré definida: {informacoesFrequencia}. Detalhes : {ex}", LogNivel.Critico, LogContexto.Frequencia));
                    throw new NegocioException(string.Format(MensagensNegocioFrequencia.Nao_foi_possivel_registrar_a_frequencia_do_dia_x,request.DataAula.ToString("dd/MM/yyyy")));
                }
            }
        }

        private static string FormatarInformacoesFrequencia(Dictionary<int, List<RegistroFrequenciaAluno>> dicionarioFrequenciaAluno, long registroFrequenciaId, long turmaId, long aulaId)
        {
            var alunosValores = dicionarioFrequenciaAluno.SelectMany(s => s.Value.Select(a => new {a.CodigoAluno, a.Valor}).ToList()).ToList();

            var informacoesFrequencia = new { registroFrequenciaId, turmaId, aulaId, alunosValores };
            
            var json = JsonConvert.SerializeObject(informacoesFrequencia, Formatting.Indented);
            
            return json;
        }

        private async Task CadastreFrequenciaAluno(Dictionary<int, List<RegistroFrequenciaAluno>> dicionario)
        {
            await repositorioRegistroFrequenciaAluno.InserirVariosComLog(dicionario[INSERIR]);

            foreach (var frequenciaAluno in dicionario[ALTERAR])
                await repositorioRegistroFrequenciaAluno.SalvarAsync(frequenciaAluno);
        }

        private async Task CadastreFrequenciaPreDefinida(Dictionary<int, List<FrequenciaPreDefinida>> dicionario)
        {
            await repositorioFrequenciaPreDefinida.InserirVarios(dicionario[INSERIR]);

            foreach (var frequenciaPreDefinida in dicionario[ALTERAR])
                await repositorioFrequenciaPreDefinida.Atualizar(frequenciaPreDefinida);
        }


        private async Task<Dictionary<int, List<RegistroFrequenciaAluno>>> ObterDicionarioFrequenciaAlunoParaPersistir(InserirRegistrosFrequenciasAlunosCommand request)
        {
            var dicionario = new Dictionary<int, List<RegistroFrequenciaAluno>>();
            var listaDeFrequenciaAlunoCadastrada = await mediator.Send(new ObterRegistroDeFrequenciaAlunoPorIdRegistroQuery(request.RegistroFrequenciaId));
            dicionario.Add(INSERIR, new List<RegistroFrequenciaAluno>());
            dicionario.Add(ALTERAR, new List<RegistroFrequenciaAluno>());

            foreach (var frequencia in request.Frequencias)
            {
                foreach (var aulaRegistrada in frequencia.Aulas)
                {
                    var frequenciaAluno = listaDeFrequenciaAlunoCadastrada.FirstOrDefault(fr => fr.NumeroAula == aulaRegistrada.NumeroAula && fr.CodigoAluno == frequencia.CodigoAluno);
                    var presenca = ObtenhaValorPresenca(frequencia.TipoFrequenciaPreDefinido, aulaRegistrada.TipoFrequencia);

                    if (frequenciaAluno != null)
                    {
                        if (frequenciaAluno.Valor != (int)presenca)
                        {
                            frequenciaAluno.Valor = (int)presenca;
                            frequenciaAluno.AulaId = request.AulaId;
                            dicionario[ALTERAR].Add(frequenciaAluno);
                        }
                    }
                    else
                    {
                        var novafrequencia = new RegistroFrequenciaAluno()
                        {
                            CodigoAluno = frequencia.CodigoAluno,
                            NumeroAula = aulaRegistrada.NumeroAula,
                            Valor = (int)presenca,
                            AulaId = request.AulaId,
                            RegistroFrequenciaId = request.RegistroFrequenciaId
                        };

                        dicionario[INSERIR].Add(novafrequencia);
                    }
                }
            }

            return dicionario;
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

        private TipoFrequencia ObtenhaValorPresenca(string tipoFrequenciaPreDefinido, string tipoFrequencia)
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

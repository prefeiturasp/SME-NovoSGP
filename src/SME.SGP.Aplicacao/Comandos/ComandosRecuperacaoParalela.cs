using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ComandosRecuperacaoParalela : IComandosRecuperacaoParalela
    {
        private readonly IConsultaRecuperacaoParalela consultaRecuperacaoParalela;
        private readonly IRepositorioRecuperacaoParalela repositorioRecuperacaoParalela;
        private readonly IRepositorioRecuperacaoParalelaPeriodoObjetivoResposta repositorioRecuperacaoParalelaPeriodoObjetivoResposta;
        private readonly IUnitOfWork unitOfWork;
        private readonly IServicoUsuario servicoUsuario;
        private readonly IServicoEol servicoEOL;
        private readonly IMediator mediator;

        public ComandosRecuperacaoParalela(IRepositorioRecuperacaoParalela repositorioRecuperacaoParalela,
            IRepositorioRecuperacaoParalelaPeriodoObjetivoResposta repositorioRecuperacaoParalelaPeriodoObjetivo,
            IConsultaRecuperacaoParalela consultaRecuperacaoParalela,
            IUnitOfWork unitOfWork,
            IServicoUsuario servicoUsuario,
            IServicoEol servicoEOL,
            IMediator mediator
            )
        {
            this.repositorioRecuperacaoParalela = repositorioRecuperacaoParalela ?? throw new ArgumentNullException(nameof(repositorioRecuperacaoParalela));
            this.repositorioRecuperacaoParalelaPeriodoObjetivoResposta = repositorioRecuperacaoParalelaPeriodoObjetivo ?? throw new ArgumentNullException(nameof(repositorioRecuperacaoParalelaPeriodoObjetivo));
            this.consultaRecuperacaoParalela = consultaRecuperacaoParalela ?? throw new ArgumentNullException(nameof(consultaRecuperacaoParalela));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<RecuperacaoParalelaListagemDto> Salvar(RecuperacaoParalelaDto recuperacaoParalelaDto)
        {
            var list = new List<RecuperacaoParalelaListagemDto>();

            //var usuarioLogado = await servicoUsuario.ObterUsuarioLogado();
            var usuarioLogin =  servicoUsuario.ObterLoginAtual();
            var usuarioPerfil = servicoUsuario.ObterPerfilAtual();


            var turmaRecuperacaoParalelaId = recuperacaoParalelaDto.Periodo.Alunos.FirstOrDefault().TurmaRecuperacaoParalelaId;
            var turmaRecuperacaoParalela = await mediator.Send(new ObterTurmaSimplesPorIdQuery(turmaRecuperacaoParalelaId));

            //var turmaCodigo = recuperacaoParalelaDto.Periodo.Alunos.FirstOrDefault().TurmaRecuperacaoParalelaId;

            var turmaPap = await servicoEOL.TurmaPossuiComponenteCurricularPAP(turmaRecuperacaoParalela.Codigo, usuarioLogin, usuarioPerfil);

            if (!turmaPap)
                throw new NegocioException("Somente é possivel realizar acompanhamento para turmas PAP");

            var turmasCodigo = recuperacaoParalelaDto.Periodo.Alunos.Select(a => a.TurmaId.ToString()).Distinct().ToArray();

            var turmas = await mediator.Send(new ObterTurmasPorCodigosQuery(turmasCodigo));

            unitOfWork.IniciarTransacao();

            foreach (var item in recuperacaoParalelaDto.Periodo.Alunos)
            {
                var turmaDoItem = turmas.FirstOrDefault(a => a.CodigoTurma == item.TurmaId.ToString());
                var recuperacaoParalela = MapearEntidade(recuperacaoParalelaDto, item, turmaDoItem.Id, turmaRecuperacaoParalela.Id, turmaDoItem.AnoLetivo);

                await repositorioRecuperacaoParalela.SalvarAsync(recuperacaoParalela);
                await repositorioRecuperacaoParalelaPeriodoObjetivoResposta.Excluir(item.Id, recuperacaoParalelaDto.Periodo.Id);
                await SalvarRespostasAluno(recuperacaoParalelaDto, item, recuperacaoParalela);
            }
            unitOfWork.PersistirTransacao();

            return await consultaRecuperacaoParalela.Listar(new Infra.FiltroRecuperacaoParalelaDto
            {
                Ordenacao = recuperacaoParalelaDto.Ordenacao,
                PeriodoId = recuperacaoParalelaDto.Periodo.Id,
                TurmaId = turmaRecuperacaoParalelaId,
                TurmaCodigo = long.Parse(turmaRecuperacaoParalela.Codigo)
            });
        }

        private static RecuperacaoParalela MapearEntidade(RecuperacaoParalelaDto recuperacaoParalelaDto, RecuperacaoParalelaAlunoDto item, long turmaId, long turmaRecuperacaoParalelaId, int anoLetivo)
        {
            return new RecuperacaoParalela
            {
                Id = item.Id,
                TurmaId = turmaId,
                AnoLetivo = anoLetivo,
                TurmaRecuperacaoParalelaId = turmaRecuperacaoParalelaId,
                Aluno_id = item.CodAluno,
                CriadoEm = recuperacaoParalelaDto.Periodo.CriadoEm ?? default,
                CriadoRF = recuperacaoParalelaDto.Periodo.CriadoRF ?? null,
                CriadoPor = recuperacaoParalelaDto.Periodo.CriadoPor ?? null
            };
        }

        private async Task SalvarRespostasAluno(RecuperacaoParalelaDto recuperacaoParalelaDto, RecuperacaoParalelaAlunoDto item, RecuperacaoParalela recuperacaoParalela)
        {
            var aluno = recuperacaoParalelaDto.Periodo.Alunos.FirstOrDefault(w => w.CodAluno == item.CodAluno);

            if (aluno == null || !aluno.Respostas.Any())
                return;

            var respostasFiltradas = aluno.Respostas.Where(x => x.RespostaId != 0);

            foreach (var resposta in respostasFiltradas)
            {
                await repositorioRecuperacaoParalelaPeriodoObjetivoResposta.SalvarAsync(new RecuperacaoParalelaPeriodoObjetivoResposta
                {
                    ObjetivoId = resposta.ObjetivoId,
                    PeriodoRecuperacaoParalelaId = recuperacaoParalelaDto.Periodo.Id,
                    RecuperacaoParalelaId = recuperacaoParalela.Id,
                    RespostaId = resposta.RespostaId
                });
            }
        }
    }
}
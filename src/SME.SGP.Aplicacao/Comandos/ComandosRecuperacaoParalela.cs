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

        public ComandosRecuperacaoParalela(IRepositorioRecuperacaoParalela repositorioRecuperacaoParalela,
            IRepositorioRecuperacaoParalelaPeriodoObjetivoResposta repositorioRecuperacaoParalelaPeriodoObjetivo,
            IConsultaRecuperacaoParalela consultaRecuperacaoParalela,
            IUnitOfWork unitOfWork,
            IServicoUsuario servicoUsuario,
            IServicoEol servicoEOL
            )
        {
            this.repositorioRecuperacaoParalela = repositorioRecuperacaoParalela ?? throw new ArgumentNullException(nameof(repositorioRecuperacaoParalela));
            this.repositorioRecuperacaoParalelaPeriodoObjetivoResposta = repositorioRecuperacaoParalelaPeriodoObjetivo ?? throw new ArgumentNullException(nameof(repositorioRecuperacaoParalelaPeriodoObjetivo));
            this.consultaRecuperacaoParalela = consultaRecuperacaoParalela ?? throw new ArgumentNullException(nameof(consultaRecuperacaoParalela));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
        }

        public async Task<RecuperacaoParalelaListagemDto> Salvar(RecuperacaoParalelaDto recuperacaoParalelaDto)
        {
            var list = new List<RecuperacaoParalelaListagemDto>();

            var usuarioLogado = await servicoUsuario.ObterUsuarioLogado();

            var turmaCodigo = recuperacaoParalelaDto.Periodo.Alunos.FirstOrDefault().TurmaRecuperacaoParalelaId;

            var turmaPap = await servicoEOL.TurmaPossuiComponenteCurricularPAP(turmaCodigo.ToString(), usuarioLogado.Login, usuarioLogado.PerfilAtual);

            if (!turmaPap)
                throw new NegocioException("Somente é possivel realizar acompanhamento para turmas PAP");
            
            unitOfWork.IniciarTransacao();

            foreach (var item in recuperacaoParalelaDto.Periodo.Alunos)
            {
                var recuperacaoParalela = MapearEntidade(recuperacaoParalelaDto, item);

                await repositorioRecuperacaoParalela.SalvarAsync(recuperacaoParalela);
                await repositorioRecuperacaoParalelaPeriodoObjetivoResposta.Excluir(item.Id, recuperacaoParalelaDto.Periodo.Id);
                await SalvarRespostasAluno(recuperacaoParalelaDto, item, recuperacaoParalela);
            }
            unitOfWork.PersistirTransacao();
            return await consultaRecuperacaoParalela.Listar(new Infra.FiltroRecuperacaoParalelaDto
            {
                Ordenacao = recuperacaoParalelaDto.Ordenacao,
                PeriodoId = recuperacaoParalelaDto.Periodo.Id,
                TurmaId = recuperacaoParalelaDto.Periodo.Alunos.FirstOrDefault().TurmaRecuperacaoParalelaId
            });
        }

        private static RecuperacaoParalela MapearEntidade(RecuperacaoParalelaDto recuperacaoParalelaDto, RecuperacaoParalelaAlunoDto item)
        {
            return new RecuperacaoParalela
            {
                Id = item.Id,
                TurmaId = item.TurmaId,
                TurmaRecuperacaoParalelaId = item.TurmaRecuperacaoParalelaId,
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
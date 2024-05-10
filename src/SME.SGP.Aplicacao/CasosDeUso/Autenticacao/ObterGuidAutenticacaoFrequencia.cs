using MediatR;
using SME.SGP.Aplicacao.Commands.Autenticacao.DeslogarSuporteUsuario;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Aplicacao.Servicos;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Net;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterGuidAutenticacaoFrequencia : AbstractUseCase, IObterGuidAutenticacaoFrequencia
    {
        private readonly IServicoAutenticacao servicoAutenticacao;

        public ObterGuidAutenticacaoFrequencia(IMediator mediator, IServicoAutenticacao servicoAutenticacao) : base(mediator)
        {
            this.servicoAutenticacao = servicoAutenticacao ?? throw new ArgumentNullException(nameof(servicoAutenticacao));
        }

        public async Task<Guid> Executar(SolicitacaoGuidAutenticacaoFrequenciaDto filtroSolicitacaoGuidAutenticacao)
        {
            var guid = Guid.NewGuid();
            var retornoAutenticacaoEol = await servicoAutenticacao.AutenticarNoEolSemSenha(filtroSolicitacaoGuidAutenticacao.Rf);
            if (!retornoAutenticacaoEol.UsuarioAutenticacaoRetornoDto.Autenticado)
                throw new NegocioException("", HttpStatusCode.Unauthorized);

            var turma = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(filtroSolicitacaoGuidAutenticacao.TurmaCodigo));
            if (turma.EhNulo())
                throw new NegocioException(MensagemNegocioTurma.TURMA_NAO_ENCONTRADA);

            var autenticacaoFrequenciaDto = new AutenticacaoFrequenciaDto { Rf = filtroSolicitacaoGuidAutenticacao.Rf, 
                                                                                  ComponenteCurricularCodigo = filtroSolicitacaoGuidAutenticacao.ComponenteCurricularCodigo, 
                                                                                  Turma = new TurmaUeDreDto() { AnoLetivo = turma.AnoLetivo,
                                                                                                        Codigo = turma.CodigoTurma,
                                                                                                        Id = turma.Id,
                                                                                                        ModalidadeCodigo = turma.ModalidadeCodigo,
                                                                                                        Nome = turma.Nome,
                                                                                                        Semestre = turma.Semestre,
                                                                                                        Tipo = turma.TipoTurma,
                                                                                                        TipoTurno = turma.TipoTurno,
                                                                                                        Historica = turma.Historica,
                                                                                                        UeId = turma.UeId,
                                                                                                        UeCodigo = turma.Ue.CodigoUe,
                                                                                                        UeNome = turma.Ue.Nome,
                                                                                                        TipoEscola = turma.Ue.TipoEscola,
                                                                                                        DreId = turma.Ue.DreId,
                                                                                                        DreCodigo = turma.Ue.Dre.CodigoDre,
                                                                                                        DreAbreviacao = turma.Ue.Dre.Abreviacao,
                                                                                                        DreNome = turma.Ue.Dre.Nome,
                                                                                                        NomeTipoUeDre = turma.ObterEscola()
                                                                                  }, 
                                                                                  UsuarioAutenticacao = retornoAutenticacaoEol };
            await mediator.Send(new SalvarCachePorValorObjetoCommand(string.Format(NomeChaveCache.AUTENTICACAO_FREQUENCIA, guid), autenticacaoFrequenciaDto, 1));
            return guid;
        }
    }
}

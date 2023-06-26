using MediatR;
using SME.SGP.Aplicacao.Commands.Autenticacao.DeslogarSuporteUsuario;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Aplicacao.Servicos;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterGuidAutenticacaoFrequenciaSGA : AbstractUseCase, IObterGuidAutenticacaoFrequenciaSGA
    {
        private readonly IServicoAutenticacao servicoAutenticacao;

        public ObterGuidAutenticacaoFrequenciaSGA(IMediator mediator, IServicoAutenticacao servicoAutenticacao) : base(mediator)
        {
            this.servicoAutenticacao = servicoAutenticacao ?? throw new ArgumentNullException(nameof(servicoAutenticacao));
        }

        public async Task<Guid> Executar(SolicitacaoGuidAutenticacaoFrequenciaSGADto filtroSolicitacaoGuidAutenticacao)
        {
            var guid = Guid.NewGuid();
            var retornoAutenticacaoEol = await servicoAutenticacao.AutenticarNoEolSemSenha(filtroSolicitacaoGuidAutenticacao.Rf);
            if (!retornoAutenticacaoEol.Item1.Autenticado)
                throw new NegocioException("", 401);

            var turma = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(filtroSolicitacaoGuidAutenticacao.TurmaCodigo));
            if (turma == null)
                throw new NegocioException(MensagemNegocioTurma.TURMA_NAO_ENCONTRADA);

            var autenticacaoFrequenciaSGADto = new AutenticacaoFrequenciaSGADto { Rf = filtroSolicitacaoGuidAutenticacao.Rf, 
                                                                                  ComponenteCurricularCodigo = filtroSolicitacaoGuidAutenticacao.ComponenteCurricularCodigo, 
                                                                                  Turma = turma, 
                                                                                  usuarioAutenticado = retornoAutenticacaoEol };
            await mediator.Send(new SalvarCachePorValorObjetoCommand(string.Format(NomeChaveCache.CHAVE_AUTENTICACAO_FREQUENCIA_SGA, guid), autenticacaoFrequenciaSGADto, 1));
            return guid;
        }
    }
}

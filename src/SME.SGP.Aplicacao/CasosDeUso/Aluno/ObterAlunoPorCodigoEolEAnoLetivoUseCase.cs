using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.Infra.Dtos.Aluno;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunoPorCodigoEolEAnoLetivoUseCase : AbstractUseCase, IObterAlunoPorCodigoEolEAnoLetivoUseCase
    {
        public ObterAlunoPorCodigoEolEAnoLetivoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<AlunoReduzidoDto> Executar(string codigoAluno, int anoLetivo, string codigoTurma, bool carregarDadosResponsaveis = false)
        {
            var alunoPorTurmaResposta = await mediator.Send(new ObterAlunoPorCodigoEolQuery(codigoAluno, anoLetivo, false, true, codigoTurma));

            if (alunoPorTurmaResposta.EhNulo())
                throw new NegocioException("Aluno não localizado");
            
            var matriculadosTurmaPAP = await BuscarAlunosTurmaPAP(new[]{codigoAluno}, anoLetivo);

            var alunoReduzido = new AlunoReduzidoDto()
            {
                Nome = !string.IsNullOrEmpty(alunoPorTurmaResposta.NomeAluno) ? alunoPorTurmaResposta.NomeAluno : alunoPorTurmaResposta.NomeSocialAluno,
                NumeroAlunoChamada = alunoPorTurmaResposta.ObterNumeroAlunoChamada(),
                DataNascimento = alunoPorTurmaResposta.DataNascimento,
                Idade = alunoPorTurmaResposta.Idade,
                DocumentoCpf = alunoPorTurmaResposta.DocumentoCpf,
                DataSituacao = alunoPorTurmaResposta.DataSituacao,
                CodigoAluno = alunoPorTurmaResposta.CodigoAluno,
                Situacao = alunoPorTurmaResposta.SituacaoMatricula,
                TurmaEscola = await OberterNomeTurmaFormatado(alunoPorTurmaResposta.CodigoTurma.ToString()),
                NomeResponsavel = alunoPorTurmaResposta.NomeResponsavel,
                TipoResponsavel = ObterTipoResponsavel(alunoPorTurmaResposta.TipoResponsavel),
                CelularResponsavel = alunoPorTurmaResposta.CelularResponsavel,
                DataAtualizacaoContato = alunoPorTurmaResposta.DataAtualizacaoContato,
                EhAtendidoAEE = await mediator.Send(new VerificaEstudantePossuiPlanoAEEPorCodigoEAnoQuery(alunoPorTurmaResposta.CodigoAluno, anoLetivo)),
                EhMatriculadoTurmaPAP = matriculadosTurmaPAP.Any(x => x.CodigoAluno.ToString() == alunoPorTurmaResposta.CodigoAluno),
                DadosResponsavelFiliacao = carregarDadosResponsaveis ? await ObterDadosResponsavelFiliacao(codigoAluno) : null
            };

            return alunoReduzido;
        }

        private async Task<IEnumerable<AlunosTurmaProgramaPapDto>> BuscarAlunosTurmaPAP(string[] alunosCodigos, int anoLetivo)
        {
            return  await mediator.Send(new ObterAlunosAtivosTurmaProgramaPapEolQuery(anoLetivo, alunosCodigos));
        }
        private async Task<string> OberterNomeTurmaFormatado(string turmaId)
        {
            var turmaNome = "";
            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(turmaId));

            if (turma.NaoEhNulo())
            {
                var nomeTurno = "";
                if (Enum.IsDefined(typeof(TipoTurnoEOL), turma.TipoTurno))
                {
                    TipoTurnoEOL tipoTurno = (TipoTurnoEOL)turma.TipoTurno;
                    nomeTurno = $"- {tipoTurno.GetAttribute<DisplayAttribute>()?.GetName()}";
                }
                
                turmaNome = $"{turma.ModalidadeCodigo.ShortName()} - {turma.Nome} {nomeTurno}";
            }

            return turmaNome;
        }

        private async Task<DadosResponsavelFiliacaoAlunoDto> ObterDadosResponsavelFiliacao(string codigoAluno)
        {
            var retorno = new DadosResponsavelFiliacaoAlunoDto();
            var responsaveisFiliacao = await mediator.Send(new ObterDadosResponsaveisAlunoEolQuery(codigoAluno));

            foreach(var responsaveis in  responsaveisFiliacao)
            {
                var telefonesFiliacao = new List<TelefonesDto>();

                AdicioneTelefone(telefonesFiliacao, responsaveis.DDDResidencial, responsaveis.NumeroResidencial, TipoTelefone.Residencial);
                AdicioneTelefone(telefonesFiliacao, responsaveis.DDDCelular, responsaveis.NumeroCelular, TipoTelefone.Celular);
                AdicioneTelefone(telefonesFiliacao, responsaveis.DDDComercial, responsaveis.NumeroComercial, TipoTelefone.Comercial);

                if (responsaveis.TipoResponsavel == TipoResponsavel.Filiacao1)
                {
                    retorno.NomeFiliacao1 = responsaveis.NomeResponsavel;
                    retorno.TelefonesFiliacao1 = telefonesFiliacao;
                }
                else
                {
                    retorno.NomeFiliacao2 = responsaveis.NomeResponsavel;
                    retorno.TelefonesFiliacao2 = telefonesFiliacao;
                }

                if (retorno.Endereco.EhNulo())
                    retorno.Endereco = responsaveis.Endereco;

                retorno.CodigoAluno = codigoAluno;
                retorno.Cpf = responsaveis.Cpf;
                retorno.Email = responsaveis.Email;
            }

            return retorno;
        }

        private void AdicioneTelefone(List<TelefonesDto> telefones, string ddd, string numero, TipoTelefone tipo)
        {
            var telefone = ObterTelefone(ddd, numero, tipo);

            if (telefone.NaoEhNulo())
                telefones.Add(telefone);
        }

        private TelefonesDto ObterTelefone(string ddd, string numero, TipoTelefone tipo)
        {
            if (!string.IsNullOrEmpty(ddd) && !string.IsNullOrEmpty(numero))
                return new TelefonesDto() { DDD = ddd, Numero = numero, TipoTelefone = tipo };

            return null;
        }

        private string ObterTipoResponsavel(string tipoResponsavel)
        {
            return !string.IsNullOrEmpty(tipoResponsavel) ?
                 ((TipoResponsavel)Enum.Parse(typeof(TipoResponsavel), tipoResponsavel)).Name() :
                 TipoResponsavel.Filiacao1.Name();
        }

    }
}

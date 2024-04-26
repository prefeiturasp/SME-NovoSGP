using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using SME.SGP.Dominio.Constantes.MensagensNegocio;

namespace SME.SGP.Dominio.Servicos
{
    public class ServicoAluno : IServicoAluno
    {
        public MarcadorFrequenciaDto ObterMarcadorAluno(AlunoPorTurmaResposta aluno, PeriodoEscolar bimestre, bool ehInfantil = false)
        {
            MarcadorFrequenciaDto marcador = null;

            var dataSituacao = $"{aluno.DataSituacao.Day.ToString().PadLeft(2, '0')}/{aluno.DataSituacao.Month.ToString().PadLeft(2, '0')}/{aluno.DataSituacao.Year}";
          
            switch (aluno.CodigoSituacaoMatricula)
            {
                case SituacaoMatriculaAluno.Ativo:
                    // Macador "Novo" durante 15 dias se iniciou depois do inicio do bimestre
                    if ((aluno.DataSituacao > bimestre.PeriodoInicio) && (aluno.DataSituacao.AddDays(15) >= DateTime.Now.Date))
                        marcador = new MarcadorFrequenciaDto()
                        {
                            Tipo = TipoMarcadorFrequencia.Novo,
                            Descricao = $"{(ehInfantil ? MensagemNegocioAluno.CRIANCA_NOVA : MensagemNegocioAluno.ESTUDANTE_NOVO)}: {MensagemNegocioAluno.DATA_MATRICULA} {dataSituacao}"
                        };
                    break;

                case SituacaoMatriculaAluno.Transferido:
                case SituacaoMatriculaAluno.TransferidoSED:
                    var detalheEscola = aluno.Transferencia_Interna ?
                                        $"{MensagemNegocioAluno.PARA_ESCOLA} {aluno.EscolaTransferencia} {MensagemNegocioAluno.E_TURMA} {aluno.TurmaTransferencia}" :
                                        MensagemNegocioAluno.PARA_OUTRAS_REDES;

                    marcador = new MarcadorFrequenciaDto()
                    {
                        Tipo = TipoMarcadorFrequencia.Transferido,
                        Descricao = $"{(ehInfantil ? MensagemNegocioAluno.CRIANCA_TRANSFERIDA : MensagemNegocioAluno.ESTUDANTE_TRANSFERIDO)}: {detalheEscola} {MensagemNegocioAluno.EM} {dataSituacao}"
                    };

                    break;

                case SituacaoMatriculaAluno.RemanejadoSaida:
                    marcador = new MarcadorFrequenciaDto()
                    {
                        Tipo = TipoMarcadorFrequencia.Remanejado,
                        Descricao = $"{(ehInfantil ? MensagemNegocioAluno.CRIANCA_REMANEJADA : MensagemNegocioAluno.ESTUDANTE_REMANEJADO)}: {MensagemNegocioAluno.TURMA} {aluno.TurmaRemanejamento} {MensagemNegocioAluno.EM} {dataSituacao}"
                    };

                    break;

                case SituacaoMatriculaAluno.Desistente:
                case SituacaoMatriculaAluno.VinculoIndevido:
                case SituacaoMatriculaAluno.Falecido:
                case SituacaoMatriculaAluno.NaoCompareceu:
                case SituacaoMatriculaAluno.Deslocamento:
                case SituacaoMatriculaAluno.Cessado:
                case SituacaoMatriculaAluno.ReclassificadoSaida:
                case SituacaoMatriculaAluno.DispensadoEdFisica:
                    marcador = new MarcadorFrequenciaDto()
                    {
                        Tipo = TipoMarcadorFrequencia.Inativo,
                        Descricao = $"{(ehInfantil ? MensagemNegocioAluno.CRIANCA_INATIVA : MensagemNegocioAluno.ESTUDANTE_INATIVO)} em {dataSituacao}"
                    };

                    break;

                default:
                    break;
            }

            return marcador;
        }
    }
}
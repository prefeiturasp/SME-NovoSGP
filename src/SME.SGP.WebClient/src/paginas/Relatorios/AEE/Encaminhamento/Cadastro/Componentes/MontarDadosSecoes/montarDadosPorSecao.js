import PropTypes from 'prop-types';
import React, { useCallback, useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import QuestionarioDinamico from '~/componentes-sgp/QuestionarioDinamico/questionarioDinamico';
import { setQuestionarioDinamicoEmEdicao } from '~/redux/modulos/questionarioDinamico/actions';
import { erros } from '~/servicos';
import ServicoEncaminhamentoAEE from '~/servicos/Paginas/Relatorios/AEE/ServicoEncaminhamentoAEE';
import AuditoriaEncaminhamento from '../AuditoriaEncaminhamento/auditoriaEncaminhamento';

const MontarDadosPorSecao = props => {
  const dispatch = useDispatch();

  const { dados, match } = props;

  const dadosEncaminhamento = useSelector(
    store => store.encaminhamentoAEE.dadosEncaminhamento
  );

  const dadosCollapseLocalizarEstudante = useSelector(
    store => store.collapseLocalizarEstudante.dadosCollapseLocalizarEstudante
  );

  const desabilitarCamposEncaminhamentoAEE = useSelector(
    store => store.encaminhamentoAEE.desabilitarCamposEncaminhamentoAEE
  );

  const [dadosQuestionarioAtual, setDadosQuestionarioAtual] = useState();

  const obterQuestionario = useCallback(async questionarioId => {
    const encaminhamentoId = match?.params?.id;
    dispatch(setQuestionarioDinamicoEmEdicao(false));
    const resposta = await ServicoEncaminhamentoAEE.obterQuestionario(
      questionarioId,
      encaminhamentoId,
      dadosCollapseLocalizarEstudante?.codigoAluno,
      dadosCollapseLocalizarEstudante?.codigoTurma
    ).catch(e => erros(e));

    if (!dadosQuestionarioAtual?.length && resposta?.data) {
      setDadosQuestionarioAtual(resposta.data);
    } else {
      setDadosQuestionarioAtual();
    }
  }, []);

  useEffect(() => {
    if (
      dados?.questionarioId &&
      dadosCollapseLocalizarEstudante?.codigoAluno &&
      dadosCollapseLocalizarEstudante?.codigoTurma
    ) {
      obterQuestionario(dados?.questionarioId);
    } else {
      setDadosQuestionarioAtual([]);
    }
  }, [dados, dadosCollapseLocalizarEstudante, obterQuestionario]);

  const validaSeDesabilitarCampo = () => {
    const encaminhamentoId = match?.params?.id;

    return (
      desabilitarCamposEncaminhamentoAEE ||
      (encaminhamentoId && !dadosEncaminhamento.podeEditar)
    );
  };

  return dados?.questionarioId && dadosQuestionarioAtual?.length ? (
    <>
      <QuestionarioDinamico
        codigoAluno={dadosCollapseLocalizarEstudante?.codigoAluno}
        codigoTurma={dadosCollapseLocalizarEstudante?.codigoTurma}
        anoLetivo={dadosCollapseLocalizarEstudante?.anoLetivo}
        dados={dados}
        dadosQuestionarioAtual={dadosQuestionarioAtual}
        desabilitarCampos={validaSeDesabilitarCampo()}
        funcaoRemoverArquivoCampoUpload={
          ServicoEncaminhamentoAEE.removerArquivo
        }
        urlUpload="v1/encaminhamento-aee/upload"
        onChangeQuestionario={() => {
          ServicoEncaminhamentoAEE.guardarSecaoEmEdicao(dados?.id);
        }}
      />
      <AuditoriaEncaminhamento dadosAuditoria={dados?.auditoria} />
    </>
  ) : (
    ''
  );
};

MontarDadosPorSecao.propTypes = {
  dados: PropTypes.oneOfType([PropTypes.object]),
  match: PropTypes.oneOfType([PropTypes.object]),
};

MontarDadosPorSecao.defaultProps = {
  dados: {},
  match: {},
};

export default MontarDadosPorSecao;

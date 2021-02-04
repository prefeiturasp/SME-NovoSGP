import PropTypes from 'prop-types';
import React, { useCallback, useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import QuestionarioDinamico from '~/componentes-sgp/QuestionarioDinamico/questionarioDinamico';
import { RotasDto } from '~/dtos';
import situacaoPlanoAEE from '~/dtos/situacaoPlanoAEE';
import { setQuestionarioDinamicoEmEdicao } from '~/redux/modulos/questionarioDinamico/actions';
import { erros, setBreadcrumbManual } from '~/servicos';
import ServicoPlanoAEE from '~/servicos/Paginas/Relatorios/AEE/ServicoPlanoAEE';

const MontarDadosPorSecao = props => {
  const dispatch = useDispatch();

  const { dados, match } = props;

  const planoAEEDados = useSelector(store => store.planoAEE.planoAEEDados);

  const dadosCollapseLocalizarEstudante = useSelector(
    store => store.collapseLocalizarEstudante.dadosCollapseLocalizarEstudante
  );

  const desabilitarCamposPlanoAEE = useSelector(
    store => store.planoAEE.desabilitarCamposPlanoAEE
  );

  const [dadosQuestionarioAtual, setDadosQuestionarioAtual] = useState();

  useEffect(() => {
    const planoId = match?.params?.id;
    if (planoId) {
      setBreadcrumbManual(
        match.url,
        'Editar Plano',
        `${RotasDto.RELATORIO_AEE_PLANO}`
      );
    }
  }, [match]);

  const obterQuestionario = useCallback(async questionarioId => {
    const planoId = match?.params?.id;
    dispatch(setQuestionarioDinamicoEmEdicao(false));
    const resposta = await ServicoPlanoAEE.obterQuestionario(
      questionarioId,
      planoId,
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
    const planoId = match?.params?.id;

    return (
      desabilitarCamposPlanoAEE ||
      (planoId && !planoAEEDados.podeEditar) ||
      planoAEEDados?.situacao === situacaoPlanoAEE.Cancelado ||
      planoAEEDados?.situacao === situacaoPlanoAEE.Encerrado
    );
  };

  return dados?.questionarioId && dadosQuestionarioAtual?.length ? (
    <QuestionarioDinamico
      codigoAluno={dadosCollapseLocalizarEstudante?.codigoAluno}
      codigoTurma={dadosCollapseLocalizarEstudante?.codigoTurma}
      anoLetivo={dadosCollapseLocalizarEstudante?.anoLetivo}
      dados={dados}
      dadosQuestionarioAtual={dadosQuestionarioAtual}
      desabilitarCampos={validaSeDesabilitarCampo()}
      funcaoRemoverArquivoCampoUpload={ServicoPlanoAEE.removerArquivo}
      urlUpload="v1/plano-aee/upload"
    />
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

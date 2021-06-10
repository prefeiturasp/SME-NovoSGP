import PropTypes from 'prop-types';
import React from 'react';
import { useSelector } from 'react-redux';
import QuestionarioDinamico from '~/componentes-sgp/QuestionarioDinamico/questionarioDinamico';
import situacaoPlanoAEE from '~/dtos/situacaoPlanoAEE';
import ServicoPlanoAEE from '~/servicos/Paginas/Relatorios/AEE/ServicoPlanoAEE';

const MontarDadosPorSecao = props => {
  const { dados, dadosQuestionarioAtual, match } = props;

  const dadosCollapseLocalizarEstudante = useSelector(
    store => store.collapseLocalizarEstudante.dadosCollapseLocalizarEstudante
  );

  const planoAEEDados = useSelector(store => store.planoAEE.planoAEEDados);

  const desabilitarCamposPlanoAEE = useSelector(
    store => store.planoAEE.desabilitarCamposPlanoAEE
  );

  const validaSeDesabilitarCampo = () => {
    const planoId = match?.params?.id;

    return (
      desabilitarCamposPlanoAEE ||
      (planoId &&
        planoAEEDados?.situacao !== situacaoPlanoAEE.EmAndamento &&
        planoAEEDados?.situacao !== situacaoPlanoAEE.Expirado &&
        planoAEEDados?.situacao !== situacaoPlanoAEE.Reestruturado)
    );
  };

  return dadosQuestionarioAtual?.length ? (
    <QuestionarioDinamico
      codigoAluno={dadosCollapseLocalizarEstudante?.codigoAluno}
      codigoTurma={dadosCollapseLocalizarEstudante?.codigoTurma}
      anoLetivo={dadosCollapseLocalizarEstudante?.anoLetivo}
      dados={dados}
      dadosQuestionarioAtual={dadosQuestionarioAtual}
      desabilitarCampos={validaSeDesabilitarCampo()}
      funcaoRemoverArquivoCampoUpload={ServicoPlanoAEE.removerArquivo}
      urlUpload="v1/plano-aee/upload"
      turmaId={dadosCollapseLocalizarEstudante?.turmaId}
    />
  ) : (
    ''
  );
};

MontarDadosPorSecao.propTypes = {
  dados: PropTypes.oneOfType([PropTypes.object]),
  match: PropTypes.oneOfType([PropTypes.object]),
  dadosQuestionarioAtual: PropTypes.oneOfType([PropTypes.object]),
};

MontarDadosPorSecao.defaultProps = {
  dados: {},
  match: {},
  dadosQuestionarioAtual: [],
};

export default MontarDadosPorSecao;

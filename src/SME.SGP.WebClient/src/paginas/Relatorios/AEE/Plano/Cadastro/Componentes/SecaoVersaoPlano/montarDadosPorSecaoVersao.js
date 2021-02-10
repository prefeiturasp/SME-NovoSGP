import PropTypes from 'prop-types';
import React, { useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import QuestionarioDinamico from '~/componentes-sgp/QuestionarioDinamico/questionarioDinamico';
import ServicoPlanoAEE from '~/servicos/Paginas/Relatorios/AEE/ServicoPlanoAEE';

const MontarDadosPorSecaoVersao = props => {
  const { versao, dados } = props;
  const [dadosQuestionarioAtual, setDadosQuestionarioAtual] = useState([]);

  const dadosCollapseLocalizarEstudante = useSelector(
    store => store.collapseLocalizarEstudante.dadosCollapseLocalizarEstudante
  );

  const obterDadosPorVersaoId = async () => {
    if (versao > 0) {
      const resposta = await ServicoPlanoAEE.obterVersaoPlanoPorId(versao);
      if (resposta?.data) {
        setDadosQuestionarioAtual(resposta?.data);
      }
    }
  };

  useEffect(() => {
    obterDadosPorVersaoId();
  }, []);

  return dadosQuestionarioAtual?.length ? (
    <QuestionarioDinamico
      codigoAluno={dadosCollapseLocalizarEstudante?.codigoAluno}
      codigoTurma={dadosCollapseLocalizarEstudante?.codigoTurma}
      anoLetivo={dadosCollapseLocalizarEstudante?.anoLetivo}
      dados={dados}
      dadosQuestionarioAtual={dadosQuestionarioAtual}
      desabilitarCampos
      funcaoRemoverArquivoCampoUpload={ServicoPlanoAEE.removerArquivo}
      urlUpload="v1/plano-aee/upload"
    />
  ) : (
    ''
  );
};

MontarDadosPorSecaoVersao.propTypes = {
  dados: PropTypes.oneOfType([PropTypes.object]),
  versao: PropTypes.oneOfType([PropTypes.number]),
};

MontarDadosPorSecaoVersao.defaultProps = {
  versao: 0,
  dados: {},
};

export default MontarDadosPorSecaoVersao;

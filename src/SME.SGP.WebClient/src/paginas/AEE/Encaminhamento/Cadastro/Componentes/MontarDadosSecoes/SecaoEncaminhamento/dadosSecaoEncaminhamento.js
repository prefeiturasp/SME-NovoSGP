import { Steps } from 'antd';
import PropTypes from 'prop-types';
import React from 'react';
import { useSelector } from 'react-redux';
import { ContainerStepsEncaminhamento } from '../../../encaminhamentoAEECadastro.css';
import DadosPorSecaoCollapse from '../dadosPorSecaoCollapse';

const { Step } = Steps;

const DadosSecaoEncaminhamento = props => {
  const { match } = props;

  const dadosCollapseLocalizarEstudante = useSelector(
    store => store.collapseLocalizarEstudante.dadosCollapseLocalizarEstudante
  );

  const dadosSecoesPorEtapaDeEncaminhamentoAEE = useSelector(
    store => store.encaminhamentoAEE.dadosSecoesPorEtapaDeEncaminhamentoAEE
  );

  return dadosCollapseLocalizarEstudante?.codigoAluno &&
    dadosSecoesPorEtapaDeEncaminhamentoAEE?.length ? (
    <ContainerStepsEncaminhamento direction="vertical" current={1}>
      {dadosSecoesPorEtapaDeEncaminhamentoAEE
        .filter(d => d.etapa === 1 || d.etapa === 2)
        .map(item => {
          return (
            <Step
              key={item?.questionarioId}
              status={item?.concluido ? 'finish' : 'process'}
              title={
                <DadosPorSecaoCollapse
                  dados={item}
                  index={item?.questionarioId}
                  match={match}
                />
              }
            />
          );
        })}
    </ContainerStepsEncaminhamento>
  ) : (
    ''
  );
};

DadosSecaoEncaminhamento.propTypes = {
  match: PropTypes.oneOfType([PropTypes.object]),
};

DadosSecaoEncaminhamento.defaultProps = {
  match: {},
};

export default DadosSecaoEncaminhamento;

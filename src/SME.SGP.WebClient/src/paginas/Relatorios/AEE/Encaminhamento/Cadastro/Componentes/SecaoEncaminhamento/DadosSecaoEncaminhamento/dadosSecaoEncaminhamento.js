import { Steps } from 'antd';
import PropTypes from 'prop-types';
import React, { useCallback, useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { setDadosSecoesPorEtapaDeEncaminhamentoAEE } from '~/redux/modulos/encaminhamentoAEE/actions';
import { erros } from '~/servicos';
import ServicoEncaminhamentoAEE from '~/servicos/Paginas/Relatorios/AEE/ServicoEncaminhamentoAEE';
import { ContainerStepsEncaminhamento } from '../../../encaminhamentoAEECadastro.css';
import DadosPorSecaoCollapse from './dadosPorSecaoCollapse';

const { Step } = Steps;

const DadosSecaoEncaminhamento = props => {
  const { match } = props;

  const dispatch = useDispatch();

  const dadosCollapseLocalizarEstudante = useSelector(
    store => store.collapseLocalizarEstudante.dadosCollapseLocalizarEstudante
  );

  const dadosSecoesPorEtapaDeEncaminhamentoAEE = useSelector(
    store => store.encaminhamentoAEE.dadosSecoesPorEtapaDeEncaminhamentoAEE
  );

  const obterSecoesPorEtapaDeEncaminhamentoAEE = useCallback(async () => {
    // TODO FAZER ENUM PARA NAO FIXAR O VALOR NA CONSULTA!
    const encaminhamentoId = match?.params?.id;
    const resposta = await ServicoEncaminhamentoAEE.obterSecoesPorEtapaDeEncaminhamentoAEE(
      1,
      encaminhamentoId
    ).catch(e => erros(e));

    if (resposta?.data) {
      dispatch(setDadosSecoesPorEtapaDeEncaminhamentoAEE(resposta.data));
    } else {
      dispatch(setDadosSecoesPorEtapaDeEncaminhamentoAEE([]));
    }
  }, [dispatch, match]);

  useEffect(() => {
    if (
      dadosCollapseLocalizarEstudante?.codigoAluno &&
      dadosCollapseLocalizarEstudante?.anoLetivo
    ) {
      obterSecoesPorEtapaDeEncaminhamentoAEE();
    } else {
      dispatch(setDadosSecoesPorEtapaDeEncaminhamentoAEE([]));
    }
  }, [
    dispatch,
    dadosCollapseLocalizarEstudante,
    obterSecoesPorEtapaDeEncaminhamentoAEE,
  ]);

  return dadosCollapseLocalizarEstudante?.codigoAluno &&
    dadosSecoesPorEtapaDeEncaminhamentoAEE?.length ? (
    <ContainerStepsEncaminhamento direction="vertical" current={1}>
      {dadosSecoesPorEtapaDeEncaminhamentoAEE.map(item => {
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

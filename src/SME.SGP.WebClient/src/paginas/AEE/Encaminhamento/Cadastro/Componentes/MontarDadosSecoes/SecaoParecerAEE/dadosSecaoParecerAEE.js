import PropTypes from 'prop-types';
import React from 'react';
import { useSelector } from 'react-redux';
import MontarDadosPorSecao from '../montarDadosPorSecao';

const DadosSecaoParecerAEE = props => {
  const { match } = props;

  const dadosCollapseLocalizarEstudante = useSelector(
    store => store.collapseLocalizarEstudante.dadosCollapseLocalizarEstudante
  );

  const dadosSecoesPorEtapaDeEncaminhamentoAEE = useSelector(
    store => store.encaminhamentoAEE.dadosSecoesPorEtapaDeEncaminhamentoAEE
  );

  return dadosCollapseLocalizarEstudante?.codigoAluno &&
    dadosSecoesPorEtapaDeEncaminhamentoAEE?.length
    ? dadosSecoesPorEtapaDeEncaminhamentoAEE
        .filter(d => d.etapa === 3)
        .map(item => {
          return <MontarDadosPorSecao dados={item} match={match} />;
        })
    : '';
};

DadosSecaoParecerAEE.propTypes = {
  match: PropTypes.oneOfType([PropTypes.object]),
};

DadosSecaoParecerAEE.defaultProps = {
  match: {},
};

export default DadosSecaoParecerAEE;

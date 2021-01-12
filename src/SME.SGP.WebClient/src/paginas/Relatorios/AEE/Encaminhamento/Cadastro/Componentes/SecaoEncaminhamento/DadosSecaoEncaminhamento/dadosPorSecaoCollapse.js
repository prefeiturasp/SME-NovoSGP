import PropTypes from 'prop-types';
import React from 'react';
import { Base } from '~/componentes';
import CardCollapse from '~/componentes/cardCollapse';
import MontarDadosPorSecao from './montarDadosPorSecao';

const DadosPorSecaoCollapse = props => {
  const { dados, index } = props;
  const { nome } = dados;

  const configCabecalho = {
    altura: '50px',
    corBorda: Base.AzulBordaCard,
  };

  const onClickCardCollapse = () => {};

  return (
    <div className="mt-2 mb-2">
      <CardCollapse
        key={`secao-${index}-collapse-key`}
        titulo={nome}
        indice={`secao-${index}-collapse-indice`}
        alt={`secao-${index}-alt`}
        onClick={onClickCardCollapse}
        configCabecalho={configCabecalho}
      >
        <MontarDadosPorSecao dados={dados} />
      </CardCollapse>
    </div>
  );
};

DadosPorSecaoCollapse.propTypes = {
  dados: PropTypes.oneOfType([PropTypes.object]),
  index: PropTypes.oneOfType([PropTypes.any]),
};

DadosPorSecaoCollapse.defaultProps = {
  dados: '',
  index: null,
};

export default DadosPorSecaoCollapse;

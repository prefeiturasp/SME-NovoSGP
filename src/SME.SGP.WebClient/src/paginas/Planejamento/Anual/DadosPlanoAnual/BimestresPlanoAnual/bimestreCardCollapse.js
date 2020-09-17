import PropTypes from 'prop-types';
import React from 'react';
import CardCollapse from '~/componentes/cardCollapse';

const BimestreCardCollapse = props => {
  const { dados } = props;
  const { bimestre } = dados;

  return (
    <CardCollapse
      key={`bimestre-${bimestre}-collapse-key`}
      titulo={`${bimestre}º Bimestre`}
      indice={`bimestre-${bimestre}-collapse-indice`}
      alt={`bimestre-${bimestre}-alt`}
      show
    >
      <>test</>
    </CardCollapse>
  );
};

BimestreCardCollapse.propTypes = {
  dados: PropTypes.oneOfType([PropTypes.object]),
};

BimestreCardCollapse.defaultProps = {
  dados: '',
};

export default BimestreCardCollapse;

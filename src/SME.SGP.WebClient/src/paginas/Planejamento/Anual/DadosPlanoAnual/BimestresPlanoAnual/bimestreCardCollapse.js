import PropTypes from 'prop-types';
import React from 'react';
import CardCollapse from '~/componentes/cardCollapse';
import TabsComponentesCorriculares from '../TabsComponentesCorriculares/tabsComponentesCorriculares';

const BimestreCardCollapse = props => {
  const { dadosBimestre } = props;
  const { bimestre } = dadosBimestre;

  return (
    <CardCollapse
      key={`bimestre-${bimestre}-collapse-key`}
      titulo={`${bimestre}º Bimestre`}
      indice={`bimestre-${bimestre}-collapse-indice`}
      alt={`bimestre-${bimestre}-alt`}
      show
    >
      <TabsComponentesCorriculares dadosBimestre={dadosBimestre} />
    </CardCollapse>
  );
};

BimestreCardCollapse.propTypes = {
  dadosBimestre: PropTypes.oneOfType([PropTypes.object]),
};

BimestreCardCollapse.defaultProps = {
  dadosBimestre: '',
};

export default BimestreCardCollapse;

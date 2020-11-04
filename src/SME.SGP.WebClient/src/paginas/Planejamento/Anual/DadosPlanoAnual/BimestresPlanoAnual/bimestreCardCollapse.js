import PropTypes from 'prop-types';
import React from 'react';
import { useDispatch } from 'react-redux';
import AlertaPeriodoEncerrado from '~/componentes-sgp/Calendario/componentes/MesCompleto/componentes/Dias/componentes/DiaCompleto/componentes/AlertaPeriodoEncerrado';
import CardCollapse from '~/componentes/cardCollapse';
import { setClicouNoBimestre } from '~/redux/modulos/anual/actions';
import TabsComponentesCorriculares from '../TabsComponentesCorriculares/tabsComponentesCorriculares';

const BimestreCardCollapse = props => {
  const { dadosBimestre } = props;
  const { bimestre } = dadosBimestre;

  const dispatch = useDispatch();

  const onClick = () => {
    dispatch(setClicouNoBimestre({ bimestre }));
  };

  return (
    <CardCollapse
      key={`bimestre-${bimestre}-collapse-key`}
      titulo={`${bimestre}º Bimestre`}
      indice={`bimestre-${bimestre}-collapse-indice`}
      alt={`bimestre-${bimestre}-alt`}
      onClick={onClick}
    >
      <AlertaPeriodoEncerrado exibir={!dadosBimestre.periodoAberto} />
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

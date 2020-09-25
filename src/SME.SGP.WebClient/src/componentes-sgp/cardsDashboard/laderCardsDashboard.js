import PropTypes from 'prop-types';
import React from 'react';
import { useSelector } from 'react-redux';
import { Loader } from '~/componentes';

const LaderCardsDashboard = ({ children }) => {
  const carregandoDadosCardsDashboard = useSelector(
    store => store.dashboard.carregandoDadosCardsDashboard
  );

  return (
    <Loader
      tip="Carregando dados do Dashboard"
      loading={carregandoDadosCardsDashboard}
    >
      {children}
    </Loader>
  );
};

LaderCardsDashboard.propTypes = {
  children: PropTypes.oneOfType([PropTypes.element, PropTypes.func]),
};

LaderCardsDashboard.defaultProps = {
  children: () => {},
};

export default LaderCardsDashboard;

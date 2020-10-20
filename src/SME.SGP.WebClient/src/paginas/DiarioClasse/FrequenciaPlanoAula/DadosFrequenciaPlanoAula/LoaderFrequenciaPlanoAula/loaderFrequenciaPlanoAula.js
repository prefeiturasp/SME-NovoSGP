import PropTypes from 'prop-types';
import React from 'react';
import { useSelector } from 'react-redux';
import { Loader } from '~/componentes';

const LoaderFrequenciaPlanoAula = ({ children }) => {
  const exibirLoaderFrequenciaPlanoAula = useSelector(
    store => store.frequenciaPlanoAula.exibirLoaderFrequenciaPlanoAula
  );

  return <Loader loading={exibirLoaderFrequenciaPlanoAula}>{children}</Loader>;
};

LoaderFrequenciaPlanoAula.propTypes = {
  children: PropTypes.oneOfType([PropTypes.element, PropTypes.func]),
};

LoaderFrequenciaPlanoAula.defaultProps = {
  children: () => {},
};

export default LoaderFrequenciaPlanoAula;

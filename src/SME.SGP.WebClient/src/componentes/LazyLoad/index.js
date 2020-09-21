import React, { Suspense } from 'react';
import t from 'prop-types';

// Componentes
import { Loader } from '~/componentes';

const ComponenteLoader = () => (
  <Loader loading tip="Carregando...">
    <br />
  </Loader>
);

function LazyLoad({ children }) {
  return <Suspense fallback={<ComponenteLoader />}>{children}</Suspense>;
}

LazyLoad.propTypes = {
  children: t.oneOfType([t.func, t.object, t.element]),
};

LazyLoad.defaultProps = {
  children: () => {},
};

export default LazyLoad;

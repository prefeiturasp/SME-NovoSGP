import React, { Suspense } from 'react';
import t from 'prop-types';

// Componentes
import { Loader } from '~/componentes';

function LazyLoad({ children }) {
  return <Suspense fallback={Loader}>{children}</Suspense>;
}

LazyLoad.propTypes = {
  children: t.oneOfType([t.func, t.object, t.element]),
};

LazyLoad.defaultProps = {
  children: () => {},
};

export default LazyLoad;

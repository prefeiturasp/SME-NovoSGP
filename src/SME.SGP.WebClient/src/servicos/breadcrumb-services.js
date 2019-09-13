import React from 'react';
import {store} from '~/redux';
import {setRotas, activeRoute} from '~/redux/modulos/navegacao/actions';

const setBreadcrumbManual = (path, breadcrumbName, parent) =>{
  const rota = {
    path,
    breadcrumbName,
    parent
  }
  store.dispatch(setRotas(rota));
  store.dispatch(activeRoute({path, breadcrumbName, parent}));
}

export {setBreadcrumbManual};

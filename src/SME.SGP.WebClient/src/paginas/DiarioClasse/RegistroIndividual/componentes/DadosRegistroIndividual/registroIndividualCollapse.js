import React from 'react';
import PropTypes from 'prop-types';
import shortid from 'shortid';

import { CardCollapse } from '~/componentes';

import DescricaoPlanejamento from './DescricaoPlanejamento/descricaoPlanejamento';

const RegistroIndividualCollapse = ({ titulo }) => {
  const idCollapse = shortid.generate();

  const onClick = () => {};

  return (
    <CardCollapse
      styles={{ height: 44 }}
      styleCardBody={{ paddingTop: 12 }}
      key={`${idCollapse}-collapse-key`}
      titulo={titulo}
      indice={`${idCollapse}-collapse-indice`}
      alt={`${idCollapse}-alt`}
      onClick={onClick}
    >
      <DescricaoPlanejamento />
    </CardCollapse>
  );
};

RegistroIndividualCollapse.propTypes = {
  titulo: PropTypes.oneOfType([PropTypes.object, PropTypes.string]),
};

RegistroIndividualCollapse.defaultProps = {
  titulo: '',
};

export default RegistroIndividualCollapse;

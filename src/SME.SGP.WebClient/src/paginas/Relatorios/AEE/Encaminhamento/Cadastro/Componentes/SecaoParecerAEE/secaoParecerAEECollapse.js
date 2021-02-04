import PropTypes from 'prop-types';
import React from 'react';
import { useSelector } from 'react-redux';
import CardCollapse from '~/componentes/cardCollapse';
import DetalhesResponsavel from '../DetalhesResponsavel/detalhesResponsavel';

const SecaoParecerAEECollapse = props => {
  const { match } = props;

  const dadosCollapseAtribuicaoResponsavel = useSelector(
    store =>
      store.collapseAtribuicaoResponsavel.dadosCollapseAtribuicaoResponsavel
  );

  return (
    <CardCollapse
      key="secao-parecer-aee-collapse-key"
      titulo="Parecer AEE"
      indice="secao-parecer-aee-collapse-indice"
      alt="secao-parecer-aee-alt"
    >
      {dadosCollapseAtribuicaoResponsavel?.rf ? (
        <>
          <DetalhesResponsavel
            rf={dadosCollapseAtribuicaoResponsavel?.rf}
            nome={dadosCollapseAtribuicaoResponsavel?.nome}
          />
        </>
      ) : (
        ''
      )}
    </CardCollapse>
  );
};

SecaoParecerAEECollapse.propTypes = {
  match: PropTypes.oneOfType([PropTypes.object]),
};

SecaoParecerAEECollapse.defaultProps = {
  match: {},
};

export default SecaoParecerAEECollapse;

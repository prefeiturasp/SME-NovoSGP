import PropTypes from 'prop-types';
import React from 'react';
import { useSelector } from 'react-redux';
import CardCollapse from '~/componentes/cardCollapse';
import DetalhesResponsavel from '../../DetalhesResponsavel/detalhesResponsavel';
import MontarDadosSecaoParecerAEE from './dadosSecaoParecerAEE';

const SecaoParecerAEECollapse = props => {
  const { match } = props;

  const dadosCollapseAtribuicaoResponsavel = useSelector(
    store =>
      store.collapseAtribuicaoResponsavel.dadosCollapseAtribuicaoResponsavel
  );

  return dadosCollapseAtribuicaoResponsavel?.codigoRF ? (
    <CardCollapse
      key="secao-parecer-aee-collapse-key"
      titulo="Parecer AEE"
      indice="secao-parecer-aee-collapse-indice"
      alt="secao-parecer-aee-alt"
      show={dadosCollapseAtribuicaoResponsavel?.codigoRF}
    >
      <DetalhesResponsavel
        codigoRF={dadosCollapseAtribuicaoResponsavel?.codigoRF}
        nomeServidor={dadosCollapseAtribuicaoResponsavel?.nomeServidor}
      />
      <MontarDadosSecaoParecerAEE match={match} />
    </CardCollapse>
  ) : (
    ''
  );
};

SecaoParecerAEECollapse.propTypes = {
  match: PropTypes.oneOfType([PropTypes.object]),
};

SecaoParecerAEECollapse.defaultProps = {
  match: {},
};

export default SecaoParecerAEECollapse;

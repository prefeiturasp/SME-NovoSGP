import React from 'react';
import { useSelector } from 'react-redux';
import CardCollapse from '~/componentes/cardCollapse';
import DetalhesResponsavel from '../../DetalhesResponsavel/detalhesResponsavel';
import MontarDadosSecaoParecerAEE from './montarDadosSecaoParecerAEE';

const SecaoParecerAEECollapse = () => {
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
    >
      <DetalhesResponsavel
        codigoRF={dadosCollapseAtribuicaoResponsavel?.codigoRF}
        nomeServidor={dadosCollapseAtribuicaoResponsavel?.nomeServidor}
      />
      <MontarDadosSecaoParecerAEE />
    </CardCollapse>
  ) : (
    ''
  );
};

export default SecaoParecerAEECollapse;

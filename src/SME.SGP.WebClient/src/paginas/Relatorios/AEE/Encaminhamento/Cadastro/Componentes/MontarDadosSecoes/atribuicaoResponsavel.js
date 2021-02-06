import React from 'react';
import { useSelector } from 'react-redux';
import CollapseAtribuicaoResponsavel from '~/componentes-sgp/CollapseAtribuicaoResponsavel/collapseAtribuicaoResponsavel';
import situacaoAEE from '~/dtos/situacaoAEE';

const AtribuicaoResponsavel = () => {
  const dadosEncaminhamento = useSelector(
    store => store.encaminhamentoAEE.dadosEncaminhamento
  );

  return dadosEncaminhamento?.situacao === situacaoAEE.AtribuicaoResponsavel ? (
    <CollapseAtribuicaoResponsavel />
  ) : (
    ''
  );
};

export default AtribuicaoResponsavel;

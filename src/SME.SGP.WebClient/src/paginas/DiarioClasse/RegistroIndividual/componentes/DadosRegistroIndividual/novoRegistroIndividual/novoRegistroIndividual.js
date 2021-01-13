import React, { useCallback, useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import shortid from 'shortid';

import { CardCollapse } from '~/componentes';

import { CONFIG_COLLAPSE_REGISTRO_INDIVIDUAL } from '~/constantes';

import { NovoRegistroIndividualConteudo } from './novoRegistroIndividualConteudo';

const NovoRegistroIndividual = () => {
  const [expandir, setExpandir] = useState(false);
  const [exibirCollapse, setExibirCollapse] = useState(false);

  const dadosAlunoObjectCard = useSelector(
    store => store.registroIndividual.dadosAlunoObjectCard
  );
  const podeRealizarNovoRegistro = useSelector(
    store => store.registroIndividual.podeRealizarNovoRegistro
  );
  const idCollapse = shortid.generate();

  useEffect(() => {
    if (podeRealizarNovoRegistro && dadosAlunoObjectCard) {
      setExpandir(true);
      setExibirCollapse(podeRealizarNovoRegistro);
    }
  }, [podeRealizarNovoRegistro, setExibirCollapse, dadosAlunoObjectCard]);

  const expandirAlternado = useCallback(() => setExpandir(!expandir), [
    expandir,
  ]);

  return (
    <>
      {exibirCollapse && (
        <div key={shortid.generate()} className="px-4 pt-4">
          <CardCollapse
            configCabecalho={CONFIG_COLLAPSE_REGISTRO_INDIVIDUAL}
            styleCardBody={{ paddingTop: 12 }}
            key={`${idCollapse}-collapse-key`}
            titulo="Novo registro individual"
            indice={`${idCollapse}-collapse-indice`}
            alt={`${idCollapse}-alt`}
            show={expandir}
            onClick={expandirAlternado}
          >
            <NovoRegistroIndividualConteudo />
          </CardCollapse>
        </div>
      )}
    </>
  );
};

export default NovoRegistroIndividual;

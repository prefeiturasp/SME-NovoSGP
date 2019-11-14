import React, { useState, useEffect } from 'react';

// Componentes
import { SelectComponent } from '~/componentes';

// Servicos
import AtribuicaoEsporadicaServico from '~/servicos/Paginas/AtribuicaoEsporadica';

function UeDropDown({ form, onChange, dreId }) {
  const [listaUes, setListaUes] = useState([]);

  useEffect(() => {
    AtribuicaoEsporadicaServico.buscarUes(dreId)
      .then(({ data }) => setListaUes(data))
      .catch(err => console.log(err));
  }, [dreId]);

  return (
    <SelectComponent
      form={form}
      className="fonte-14"
      onChange={onChange}
      lista={listaUes}
      valueOption="codigo"
      containerVinculoId="containerFiltro"
      valueText="nome"
      //valueSelect={dreSelecionada && `${dreSelecionada}`}
      placeholder="Unidade Escolar (UE)"
      //disabled={campoDreDesabilitado}
    />
  );
}

export default UeDropDown;

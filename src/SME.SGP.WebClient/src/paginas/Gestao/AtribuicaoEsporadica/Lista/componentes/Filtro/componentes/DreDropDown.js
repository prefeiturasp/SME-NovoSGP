import React, { useState, useEffect } from 'react';

// Componentes
import { SelectComponent } from '~/componentes';

// Servicos
import AtribuicaoEsporadicaServico from '~/servicos/Paginas/AtribuicaoEsporadica';

function DreDropDown({ form, onChange }) {
  const [listaDres, setListaDres] = useState([]);

  useEffect(() => {
    AtribuicaoEsporadicaServico.buscarDres()
      .then(({ data }) => setListaDres(data))
      .catch(err => console.log(err));
  }, []);

  return (
    <SelectComponent
      form={form}
      className="fonte-14"
      onChange={onChange}
      lista={listaDres}
      valueOption="codigo"
      containerVinculoId="containerFiltro"
      valueText="nome"
      //valueSelect={dreSelecionada && `${dreSelecionada}`}
      placeholder="Diretoria Regional De Educação (DRE)"
      //disabled={campoDreDesabilitado}
    />
  );
}

export default DreDropDown;

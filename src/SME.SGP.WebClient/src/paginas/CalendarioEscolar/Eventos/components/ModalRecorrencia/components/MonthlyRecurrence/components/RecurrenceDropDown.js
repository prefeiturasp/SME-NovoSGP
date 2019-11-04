import React from 'react';

// Styles
import styled from 'styled-components';

// Components
import SelectComponent from '~/componentes/select';

const SelectWrapper = styled.div`
  .ant-select-selection {
    width: 150px;
  }
`;

function RecurrenceDropDown({ onChange, selected, form }) {
  const items = [
    {
      desc: 'No dia',
      valor: '0',
    },
    {
      desc: 'Primeiro',
      valor: '1',
    },
    {
      desc: 'Segundo',
      valor: '2',
    },
    {
      desc: 'Terceiro',
      valor: '3',
    },
    {
      desc: 'Quarto',
      valor: '4',
    },
    {
      desc: 'Último',
      valor: '5',
    },
  ];

  return (
    <SelectWrapper>
      <SelectComponent
        className="fonte-14"
        onChange={onChange}
        lista={items}
        valueOption="valor"
        valueText="desc"
        valueSelect={selected}
        placeholder="Selecione a recorrência"
        form={form}
        name="padraoRecorrencia"
      />
    </SelectWrapper>
  );
}

export default RecurrenceDropDown;

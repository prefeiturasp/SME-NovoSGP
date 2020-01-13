import React from 'react';
import shortid from 'shortid';
import PropTypes from 'prop-types';

// Components
import LabelDia from './components/LabelDia';

function DiasDaSemana({ onChange, currentState }) {
  const days = [
    { legenda: 'D', valor: '0', descricao: 'Domingo' },
    { legenda: 'S', valor: '1', descricao: 'Segunda-feira' },
    { legenda: 'T', valor: '2', descricao: 'Terça-feira' },
    { legenda: 'Q', valor: '3', descricao: 'Quarta-feira' },
    { legenda: 'Q', valor: '4', descricao: 'Quinta-feira' },
    { legenda: 'S', valor: '5', descricao: 'Sexta-feira' },
    { legenda: 'S', valor: '6', descricao: 'Sábado' },
  ];

  return (
    <>
      {days.map(valor => (
        <LabelDia
          isActive={
            currentState && currentState.some(x => x.valor === valor.valor)
          }
          onChange={onChange}
          key={shortid.generate()}
          data={valor}
        />
      ))}
    </>
  );
}

DiasDaSemana.defaultProps = {
  currentState: [],
  onChange: () => {},
};

DiasDaSemana.propTypes = {
  currentState: PropTypes.oneOfType([PropTypes.array, PropTypes.object]),
  onChange: PropTypes.func,
};

export default DiasDaSemana;

import React from 'react';
import { useSelector } from 'react-redux';
import { TipoComponente } from './TipoComponente';

const LinhaJustificativa = props => {
  const { index, tipoComponente } = props;
  const conselhoClasseStore = useSelector(state => state.conselhoClasse);
  const linha =
    tipoComponente === TipoComponente.Componente
      ? conselhoClasseStore.notasJustificativas.componentes[index]
      : conselhoClasseStore.notasJustificativas.componentesRegencia[index];

  return (
    <tr hidden={linha ? !linha.habilitado : true}>
      <td style={{ height: '100px' }} colSpan="4">
        Conteudo Justificativa
      </td>
    </tr>
  );
};

export default LinhaJustificativa;

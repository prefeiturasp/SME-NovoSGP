import React, { useMemo } from 'react';
import shortid from 'shortid';
import t from 'prop-types';

// Componentes
import { Colors } from '~/componentes';

// Estilos
import { Botao } from '../styles';

function LabelAulaEvento({ dadosEvento }) {
  const tipoEventoMemo = useMemo(() => {
    const { ehAula, ehAulaCJ, tipoEvento } = dadosEvento;

    if (ehAula && !ehAulaCJ) return 'Aula';
    if (ehAula && ehAulaCJ) return 'CJ';

    return tipoEvento;
  }, [dadosEvento]);

  return (
    <Botao
      id={shortid.generate()}
      label={tipoEventoMemo}
      color={
        (tipoEventoMemo === 'Aula' && Colors.Roxo) ||
        (tipoEventoMemo === 'CJ' && Colors.Laranja) ||
        Colors.CinzaBotao
      }
      className="w-100"
      height={dadosEvento.ehAula ? '38px' : 'auto'}
      border
      steady
    />
  );
}

LabelAulaEvento.propTypes = {
  dadosEvento: t.oneOfType([t.any]),
};

LabelAulaEvento.defaultProps = {
  dadosEvento: {},
};

export default LabelAulaEvento;

import React from 'react';
import t from 'prop-types';

// Ant
import { Tooltip } from 'antd';

// Componentes
import { Button, Colors } from '~/componentes';

// Estilos
import { CabecalhoDetalhes } from '../../style';

function Cabecalho({
  titulo,
  retraido,
  desabilitarAnterior,
  desabilitarProximo,
  onClickCollapse,
  onClickAnterior,
  onClickProximo,
}) {

  const clicouEnter = e => e.keyCode == 13;

  return (
    <CabecalhoDetalhes>
      <div className="titulo">
        <Tooltip title={`${retraido ? `Expandir alunos` : `Retrair alunos`}`}>
          <span
            className={`botaoCollapse ${retraido && `retraido`}`}
            role="button"
            tabIndex={0}
            onKeyDown={e => clicouEnter(e) ? onClickCollapse() : ''}
            onClick={() => onClickCollapse()}
          >
            <i className="fas fa-chevron-left" />
          </span>
        </Tooltip>
        <span>{titulo}</span>
      </div>
      <div className="botoes">
        <div>
          <Button
            id="anteriorBtn"
            label="Anterior"
            color={Colors.Roxo}
            className="ml-auto attached right"
            bold
            onClick={() => onClickAnterior()}
            border
            disabled={desabilitarAnterior}
          />
        </div>
        <div>
          <Button
            id="proximoBtn"
            label="Próximo"
            color={Colors.Roxo}
            className="ml-auto attached right"
            bold
            onClick={() => onClickProximo()}
            border
            disabled={desabilitarProximo}
          />
        </div>
      </div>
    </CabecalhoDetalhes>
  );
}

Cabecalho.propTypes = {
  titulo: t.string,
  retraido: t.bool,
  desabilitarAnterior: t.bool,
  desabilitarProximo: t.bool,
  onClickCollapse: t.func,
  onClickAnterior: t.func,
  onClickProximo: t.func,
};

Cabecalho.defaultProps = {
  titulo: 'Sem título',
  retraido: false,
  desabilitarAnterior: false,
  desabilitarProximo: false,
  onClickCollapse: () => {},
  onClickAnterior: () => {},
  onClickProximo: () => {},
};

export default Cabecalho;

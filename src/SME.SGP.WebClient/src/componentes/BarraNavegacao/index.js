import React from 'react';
import t from 'prop-types';
import shortid from 'shortid';

// Ant
import { Tooltip } from 'antd';

// Componentes
import { Button, Colors, Base } from '~/componentes';

// Estilos
import { Container } from './styles';

function BarraNavegacao({ itens, itemAtivo, onChangeItem }) {
  return (
    <Container>
      <div>
        <Button
          id="anteriorBtn"
          label="Anterior"
          color={Colors.Roxo}
          className="ml-auto attached left"
          bold
          onClick={() => onChangeItem(itens[itens.indexOf(itemAtivo) - 1])}
          border
          disabled={itens.length < 1 || itens.indexOf(itemAtivo) === 0}
        />
      </div>
      <div className="conteudo">
        {itens.length ? (
          itens.map(item => (
            <Tooltip
              className="ttpItemNavegacao"
              key={shortid.generate()}
              title={item.descricao}
            >
              <div
                onClick={() => onChangeItem(item)}
                onKeyPress={() => item}
                role="button"
                tabIndex="0"
                className={`itemNavegacao ${
                  item.id === itemAtivo.id ? 'ativo' : ''
                }`}
              />
            </Tooltip>
          ))
        ) : (
          <span style={{ color: Base.CinzaBotao }}>Sem dados</span>
        )}
      </div>
      <div>
        <Button
          id="proximoBtn"
          label="Próximo"
          color={Colors.Roxo}
          className="ml-auto attached right"
          bold
          onClick={() => onChangeItem(itens[itens.indexOf(itemAtivo) + 1])}
          border
          disabled={
            itens.length < 1 || itens.indexOf(itemAtivo) === itens.length - 1
          }
        />
      </div>
    </Container>
  );
}

BarraNavegacao.propTypes = {
  itens: t.oneOfType([t.any]),
  itemAtivo: t.oneOfType([t.any]),
  onChangeItem: t.func,
  id: t.number,
};

BarraNavegacao.defaultProps = {
  itens: [],
  itemAtivo: {},
  onChangeItem: () => {},
  id: 0,
};

export default BarraNavegacao;

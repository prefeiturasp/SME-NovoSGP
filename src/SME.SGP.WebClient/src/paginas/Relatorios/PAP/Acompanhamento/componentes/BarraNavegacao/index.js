import React, { useEffect } from 'react';
import t from 'prop-types';
import shortid from 'shortid';

// Ant
import { Tooltip } from 'antd';

// Componentes
import { Button, Colors, Base } from '~/componentes';

// Estilos
import { Container } from './styles';

function BarraNavegacao({ objetivos, objetivoAtivo, onChangeObjetivo }) {
  useEffect(() => {
    console.log('Objetivos BarraNavegação: ', objetivos);
  }, [objetivos]);
  return (
    <Container>
      <div>
        <Button
          label="Anterior"
          color={Colors.Roxo}
          className="ml-auto attached left"
          bold
          onClick={() => null}
          border
          disabled={objetivos.length < 1}
        />
      </div>
      <div className="conteudo">
        {objetivos.length ? (
          objetivos.map(item => (
            <Tooltip
              className="ttpItemNavegacao"
              key={shortid.generate()}
              title={item.descricao}
            >
              <div
                onClick={() => onChangeObjetivo(item)}
                onKeyPress={() => item}
                role="button"
                tabIndex="0"
                className={`itemNavegacao ${
                  item.id === objetivoAtivo.id ? 'ativo' : ''
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
          label="Próximo"
          color={Colors.Roxo}
          className="ml-auto attached right"
          bold
          onClick={() => null}
          border
          disabled={objetivos.length < 1}
        />
      </div>
    </Container>
  );
}

BarraNavegacao.propTypes = {
  objetivos: t.oneOfType([t.any]),
  objetivoAtivo: t.oneOfType([t.any]),
  onChangeObjetivo: t.func,
};

BarraNavegacao.defaultProps = {
  objetivos: [],
  objetivoAtivo: null,
  onChangeObjetivo: () => {},
};

export default BarraNavegacao;

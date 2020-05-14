import React, { useEffect, useState } from 'react';
import t from 'prop-types';
import shortid from 'shortid';

// Ant
import { Tooltip } from 'antd';

// Componentes
import { Button, Colors, Base } from '~/componentes';

// Estilos
import { Container } from './styles';

function BarraNavegacao({
  objetivos,
  objetivoAtivo,
  onChangeObjetivo,
  somenteConsulta,
}) {
  return (
    <Container>
      <div>
        <Button
          id="anteriorBtn"
          label="Anterior"
          color={Colors.Roxo}
          className="ml-auto attached left"
          bold
          onClick={() =>
            onChangeObjetivo(objetivos[objetivos.indexOf(objetivoAtivo) - 1])
          }
          border
          disabled={
            somenteConsulta ||
            objetivos.length < 1 ||
            objetivos.indexOf(objetivoAtivo) === 0
          }
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
                disabled={somenteConsulta}
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
          id="proximoBtn"
          label="Próximo"
          color={Colors.Roxo}
          className="ml-auto attached right"
          bold
          onClick={() =>
            onChangeObjetivo(objetivos[objetivos.indexOf(objetivoAtivo) + 1])
          }
          border
          disabled={
            somenteConsulta ||
            objetivos.length < 1 ||
            objetivos.indexOf(objetivoAtivo) === objetivos.length - 1
          }
        />
      </div>
    </Container>
  );
}

BarraNavegacao.propTypes = {
  objetivos: t.oneOfType([t.any]),
  objetivoAtivo: t.oneOfType([t.any]),
  onChangeObjetivo: t.func,
  id: t.number,
};

BarraNavegacao.defaultProps = {
  objetivos: [],
  objetivoAtivo: {},
  onChangeObjetivo: () => {},
  id: 0,
};

export default BarraNavegacao;

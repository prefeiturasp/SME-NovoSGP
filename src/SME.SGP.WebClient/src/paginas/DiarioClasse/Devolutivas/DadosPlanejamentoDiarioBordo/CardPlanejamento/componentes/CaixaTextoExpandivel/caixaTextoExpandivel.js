import React from 'react';
import PropTypes from 'prop-types';
import moment from 'moment';

import { JoditEditor } from '~/componentes';

import {
  BotaoEstilizado,
  Container,
  EditorPlanejamento,
  IframeStyle,
} from './caixaTextoExpandivel.css';

const CaixaTextoExpandivel = ({
  dadosPlanejamentos,
  totalRegistrosSelecionado,
}) => {
  console.log('ddd', dadosPlanejamentos);

  return (
    <>
      {dadosPlanejamentos.items.map(item => (
        <Container
          className={`col-${totalRegistrosSelecionado >= 4 ? 6 : 12} mb-4`}
        >
          <div className="card">
            <div className="card-header d-flex">
              <div>
                {`${
                  item.data ? moment(item.data).format('L') : ''
                } - Planejamento`}
              </div>
              <div>
                <BotaoEstilizado
                  id="btn-expandir"
                  icon="expand-alt"
                  iconType="fas"
                  onClick={() => {}}
                  height="13px"
                  width="13px"
                />
              </div>
            </div>
            <div className="card-body">
              <EditorPlanejamento>
                {totalRegistrosSelecionado === '1' ? (
                  <JoditEditor
                    id="planejamento-diario-bordo-um"
                    value={item.planejamento}
                    removerToolbar
                    readonly
                    height="560px"
                    iframeStyle={IframeStyle}
                  />
                ) : (
                  <JoditEditor
                    id="planejamento-diario-bordo"
                    value={item.planejamento}
                    removerToolbar
                    readonly
                    height="260px"
                    iframeStyle={IframeStyle}
                  />
                )}
              </EditorPlanejamento>
            </div>
          </div>
        </Container>
      ))}
    </>
  );
};

CaixaTextoExpandivel.defaultProps = {
  dadosPlanejamentos: {},
  totalRegistrosSelecionado: '',
};

CaixaTextoExpandivel.propTypes = {
  dadosPlanejamentos: PropTypes.instanceOf(PropTypes.object),
  totalRegistrosSelecionado: PropTypes.string,
};

export default CaixaTextoExpandivel;

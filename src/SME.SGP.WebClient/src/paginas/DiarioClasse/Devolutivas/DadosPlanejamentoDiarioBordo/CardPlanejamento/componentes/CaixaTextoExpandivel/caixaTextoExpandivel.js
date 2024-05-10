import React, { useEffect, useState } from 'react';
import PropTypes from 'prop-types';
import moment from 'moment';
import { useDispatch, useSelector } from 'react-redux';

import { JoditEditor } from '~/componentes';

import {
  setPlanejamentoExpandido,
  setPlanejamentoSelecionado,
} from '~/redux/modulos/devolutivas/actions';

import {
  BotaoEstilizado,
  Container,
  EditorPlanejamento,
  FundoEditor,
  IframeStyle,
  TextoSimples,
} from './caixaTextoExpandivel.css';

const CaixaTextoExpandivel = ({ item }) => {
  const [icone, setIcone] = useState('expand-alt');

  const dadosPlanejamentos = useSelector(
    store => store.devolutivas.dadosPlanejamentos
  );

  const planejamentoExpandido = useSelector(
    store => store.devolutivas.planejamentoExpandido
  );

  const numeroRegistros = useSelector(
    store => store.devolutivas.numeroRegistros
  );

  const totalRegistros = Number(
    numeroRegistros || dadosPlanejamentos?.totalRegistros || 4
  );

  const dispatch = useDispatch();

  const cliqueAlternado = () => {
    dispatch(setPlanejamentoExpandido(!planejamentoExpandido));
    dispatch(setPlanejamentoSelecionado(item));
  };

  useEffect(() => {
    if (!planejamentoExpandido) {
      dispatch(setPlanejamentoSelecionado([]));
      setIcone('expand-alt');
      return;
    }
    setIcone('compress-alt');
  }, [dispatch, planejamentoExpandido]);

  return (
    <Container
      className={`col-${
        totalRegistros >= 4 && !planejamentoExpandido ? 6 : 12
      } mb-4`}
    >
      <div className="card">
        <div className="card-header d-flex">
          <div>
            {`${item.data ? moment(item.data).format('L') : ''} - Planejamento`}
          </div>
          {totalRegistros > 1 && (
            <div>
              <BotaoEstilizado
                id="btn-expandir"
                icon={icone}
                iconType="fas"
                onClick={() => cliqueAlternado(item)}
                height="13px"
                width="13px"
              />
            </div>
          )}
        </div>
        <div className="card-body">
          <EditorPlanejamento>
            {totalRegistros === 1 || planejamentoExpandido ? (
              <FundoEditor>
                <JoditEditor
                  id="planejamento-diario-bordo-um"
                  value={item.planejamento}
                  removerToolbar
                  readonly
                  height="560px"
                  iframeStyle={IframeStyle}
                />
              </FundoEditor>
            ) : (
              <TextoSimples>
                <div>{item.planejamentoSimples}</div>
              </TextoSimples>
            )}
          </EditorPlanejamento>
        </div>
      </div>
    </Container>
  );
};

CaixaTextoExpandivel.defaultProps = {
  item: {},
};

CaixaTextoExpandivel.propTypes = {
  item: PropTypes.oneOfType([PropTypes.object]),
};

export default CaixaTextoExpandivel;

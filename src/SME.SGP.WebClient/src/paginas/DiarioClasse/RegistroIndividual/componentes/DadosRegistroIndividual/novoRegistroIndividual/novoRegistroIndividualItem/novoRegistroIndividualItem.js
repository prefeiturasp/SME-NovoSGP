import React, { useCallback, useEffect, useState } from 'react';
import PropTypes from 'prop-types';
import { useDispatch } from 'react-redux';

import { Auditoria, CampoData, Editor } from '~/componentes';
import { setRegistroIndividualEmEdicao } from '~/redux/modulos/registroIndividual/actions';

const NovoRegistroIndividualItem = React.memo(
  ({ dadosPrincipaisRegistroIndividual }) => {
    const [data, setData] = useState();

    const auditoria =
      dadosPrincipaisRegistroIndividual?.registroIndividual?.auditoria;

    const dispatch = useDispatch();

    const onChange = useCallback(e => {
      console.log('e ==>', e);
      dispatch(setRegistroIndividualEmEdicao(true));

      // TODO Verificar para salvar dados editados no redux separada do atual para melhorar a performance!
      // const dados = { ...dadosBimestrePlanoAnual };
      // dados.componentes.forEach(item => {
      //   if (
      //     String(item.componenteCurricularId) ===
      //     String(tabAtualComponenteCurricular.codigoComponenteCurricular)
      //   ) {
      //     item.descricao = valorNovo;
      //     item.emEdicao = true;
      //   }
      // });
      // dispatch(setDadosBimestresPlanoAnual(dados));
    }, []);

    const validarSeTemErro = valorEditado => {
      // const { planoAnualEmEdicao } = planoAnual;

      const ehInvalido = false;
      // if (planoAnualEmEdicao) {
      //   if (!valorEditado) {
      //     ehInvalido = true;
      //   }
      // }

      return ehInvalido;
    };

    useEffect(() => {
      if (!data) {
        setData(window.moment());
      }
    }, [data]);

    return (
      <>
        <div className="col-3 p-0 pb-2">
          <CampoData
            name="data"
            placeholder="Selecione"
            valor={data}
            formatoData="DD/MM/YYYY"
            onChange={e => setData(e)}
          />
        </div>
        <div className="pt-1">
          <Editor
            validarSeTemErro={validarSeTemErro}
            mensagemErro="Campo obrigatório"
            id="editor"
            inicial={
              dadosPrincipaisRegistroIndividual?.registroIndividual || ''
            }
            onChange={e => onChange(e)}
          />
          {auditoria && (
            <div className="row">
              <Auditoria
                criadoEm={auditoria.criadoEm}
                criadoPor={auditoria.criadoPor}
                criadoRf={auditoria.criadoRF}
                alteradoPor={auditoria.alteradoPor}
                alteradoEm={auditoria.alteradoEm}
                alteradoRf={auditoria.alteradoRF}
              />
            </div>
          )}
        </div>
      </>
    );
  }
);

NovoRegistroIndividualItem.propTypes = {
  dadosPrincipaisRegistroIndividual: PropTypes.oneOfType([
    PropTypes.object,
    PropTypes.string,
  ]),
};

NovoRegistroIndividualItem.defaultProps = {
  dadosPrincipaisRegistroIndividual: {},
};

export default NovoRegistroIndividualItem;

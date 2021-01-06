import React, { useCallback, useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';

import { Auditoria, CampoData, Editor } from '~/componentes';
import {
  setRegistroIndividualEmEdicao,
  setDadosParaSalvarNovoRegistro,
} from '~/redux/modulos/registroIndividual/actions';
import { erros, ServicoRegistroIndividual } from '~/servicos';

const NovoRegistroIndividualItem = React.memo(({ valorInicial, idSecao }) => {
  const [data, setData] = useState();

  const {
    componenteCurricularSelecionado,
    dadosAlunoObjectCard,
    dadosPrincipaisRegistroIndividual,
  } = useSelector(state => state.registroIndividual);
  const { turmaSelecionada } = useSelector(state => state.usuario);
  const turmaId = turmaSelecionada?.id || 0;
  const auditoria =
    dadosPrincipaisRegistroIndividual?.registroIndividual?.auditoria;
  const dispatch = useDispatch();

  const obterRegistroIndividualPorData = useCallback(async () => {
    const dataAtual = data?.format('MM-DD-YYYY');
    if (dataAtual) {
      const retorno = await ServicoRegistroIndividual.obterRegistroIndividualPorData(
        {
          alunoCodigo: dadosAlunoObjectCard.codigoEOL,
          componenteCurricular: componenteCurricularSelecionado,
          data: dataAtual,
          turmaId,
        }
      ).catch(e => erros(e));
      if (retorno?.data) {
        // dispatch(setDadosPrincipaisRegistroIndividual(retorno.data));
      }
    }
  }, [
    // dispatch,
    componenteCurricularSelecionado,
    dadosAlunoObjectCard,
    turmaId,
    data,
  ]);

  useEffect(() => {
    const temDadosAlunos = Object.keys(dadosAlunoObjectCard).length;
    if (
      temDadosAlunos &&
      !dadosPrincipaisRegistroIndividual?.podeRealizarNovoRegistro
    ) {
      obterRegistroIndividualPorData();
    }
  }, [
    dadosAlunoObjectCard,
    dadosPrincipaisRegistroIndividual,
    obterRegistroIndividualPorData,
  ]);

  const onChange = useCallback(
    valor => {
      dispatch(
        setDadosParaSalvarNovoRegistro({
          id: idSecao,
          valor,
        })
      );
      dispatch(setRegistroIndividualEmEdicao(true));
    },
    [dispatch, idSecao]
  );

  const validarSeTemErro = valorEditado => {
    return !valorEditado;
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
          onChange={valor => setData(valor)}
        />
      </div>
      <div className="pt-1">
        <Editor
          validarSeTemErro={validarSeTemErro}
          mensagemErro="Campo obrigatório"
          id={`secao-${idSecao}-editor`}
          inicial={valorInicial}
          onChange={onChange}
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
});

export default NovoRegistroIndividualItem;

import React, { useCallback, useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';

import { Auditoria, CampoData } from '~/componentes';
import Editor from '~/componentes/editor/editor';

import { ServicoRegistroIndividual } from '~/servicos';

import { setDadosPrincipaisRegistroIndividual } from '~/redux/modulos/registroIndividual/actions';

const RegistrosAnterioresItem = React.memo(() => {
  const [data, setData] = useState();

  const componenteCurricularSelecionado = useSelector(
    state => state.registroIndividual.componenteCurricularSelecionado
  );
  const dadosAlunoObjectCard = useSelector(
    store => store.registroIndividual.dadosAlunoObjectCard
  );
  const dadosPrincipaisRegistroIndividual = useSelector(
    store => store.registroIndividual.dadosPrincipaisRegistroIndividual
  );
  const { turmaSelecionada } = useSelector(state => state.usuario);
  const { turma } = turmaSelecionada;
  const turmaId = turma || 0;

  const dispatch = useDispatch();

  const obterRegistroIndividualPorData = useCallback(async () => {
    const dataFormatada = data?.format('MM-DD-YYYY');

    if (dataFormatada) {
      const retorno = await ServicoRegistroIndividual.obterRegistroIndividualPorData(
        {
          alunoCodigo: dadosAlunoObjectCard.codigoEOL,
          componenteCurricular: componenteCurricularSelecionado,
          data: dataFormatada,
          turmaId,
        }
      );
      if (retorno?.data) {
        dispatch(setDadosPrincipaisRegistroIndividual(retorno.data));
      }
    }
  }, [
    dispatch,
    componenteCurricularSelecionado,
    dadosAlunoObjectCard,
    data,
    turmaId,
  ]);

  useEffect(() => {
    if (Object.keys(dadosAlunoObjectCard).length) {
      obterRegistroIndividualPorData();
    }
  }, [obterRegistroIndividualPorData, dadosAlunoObjectCard]);

  const onChange = useCallback(valorNovo => {
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
    // if (servicoSalvarPlanoAnual.campoInvalido(valorEditado)) {
    //   return true;
    // }
    return false;
  };

  const obterAuditoria = () => {
    const auditoria =
      dadosPrincipaisRegistroIndividual?.registroIndividual?.auditoria;
    if (auditoria) {
      return (
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
      );
    }
    return '';
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
          inicial={dadosPrincipaisRegistroIndividual?.registroIndividual || ''}
          onChange={v => {
            // if (
            //   !planoAnualSomenteConsulta &&
            //   periodoAberto
            // ) {
            //   dispatch(setPlanoAnualEmEdicao(true));
            //   onChange(v);
            // }
          }}
        />
        {obterAuditoria()}
      </div>
    </>
  );
});

export default RegistrosAnterioresItem;

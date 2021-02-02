import React, { useCallback, useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import DetalhesAluno from '~/componentes/Alunos/Detalhes';
import { erros } from '~/servicos';
import ServicoEncaminhamentoAEE from '~/servicos/Paginas/Relatorios/AEE/ServicoEncaminhamentoAEE';
import { Loader } from '~/componentes';
import { setDadosEstudanteObjectCardEncaminhamento } from '~/redux/modulos/encaminhamentoAEE/actions';

const ObjectCardEncaminhamento = () => {
  const dispatch = useDispatch();

  const dadosCollapseLocalizarEstudante = useSelector(
    store => store.collapseLocalizarEstudante.dadosCollapseLocalizarEstudante
  );

  const dadosEstudanteObjectCardEncaminhamento = useSelector(
    store => store.encaminhamentoAEE.dadosEstudanteObjectCardEncaminhamento
  );

  const [exibirLoader, setExibirLoader] = useState(false);

  const obterDadosEstudante = useCallback(async () => {
    const { codigoAluno, anoLetivo } = dadosCollapseLocalizarEstudante;

    setExibirLoader(true);
    const resultado = await ServicoEncaminhamentoAEE.obterDadosEstudante(
      codigoAluno,
      anoLetivo
    )
      .catch(e => erros(e))
      .finally(() => setExibirLoader(false));

    if (resultado?.data) {
      const aluno = {
        ...resultado.data,
        codigoEOL: resultado.data.codigoAluno,
        numeroChamada: resultado.data.numeroAlunoChamada,
        turma: resultado.data.turmaEscola,
      };
      dispatch(setDadosEstudanteObjectCardEncaminhamento(aluno));
    }
  }, [dispatch, dadosCollapseLocalizarEstudante]);

  useEffect(() => {
    if (!dadosEstudanteObjectCardEncaminhamento?.codigoEOL) {
      if (
        dadosCollapseLocalizarEstudante?.codigoAluno &&
        dadosCollapseLocalizarEstudante?.anoLetivo
      ) {
        obterDadosEstudante();
      } else {
        dispatch(setDadosEstudanteObjectCardEncaminhamento());
      }
    }
  }, [
    dispatch,
    dadosCollapseLocalizarEstudante,
    dadosEstudanteObjectCardEncaminhamento,
    obterDadosEstudante,
  ]);

  return (
    <Loader loading={exibirLoader}>
      <DetalhesAluno
        dados={dadosEstudanteObjectCardEncaminhamento}
        exibirBotaoImprimir={false}
        exibirFrequencia={false}
      />
    </Loader>
  );
};

export default ObjectCardEncaminhamento;

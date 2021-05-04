import React, { useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';

import { DetalhesAluno } from '~/componentes';
import { setMostrarMensagemSemHistorico } from '~/redux/modulos/registroIndividual/actions';

import { erros, ServicoRegistroIndividual, sucesso } from '~/servicos';

const ObjectCardRegistroIndividual = () => {
  const [desabilitarBotaoImprimir, setDesabilitarBotaoImprimir] = useState();
  const dadosAlunoObjectCard = useSelector(
    store => store.registroIndividual.dadosAlunoObjectCard
  );

  const desabilitarCampos = useSelector(
    state => state.registroIndividual.desabilitarCampos
  );

  const dataInicioImpressaoRegistrosAnteriores = useSelector(
    state => state.registroIndividual.dataInicioImpressaoRegistrosAnteriores
  );

  const dataFimImpressaoRegistrosAnteriores = useSelector(
    state => state.registroIndividual.dataFimImpressaoRegistrosAnteriores
  );

  const dadosRegistroAtual = useSelector(
    store => store.registroIndividual.dadosRegistroAtual
  );

  const dadosPrincipaisRegistroIndividual = useSelector(
    store => store.registroIndividual.dadosPrincipaisRegistroIndividual
  );

  const turmaSelecionada = useSelector(state => state.usuario.turmaSelecionada);

  const { codigoEOL } = dadosAlunoObjectCard;
  const dispatch = useDispatch();

  const gerar = async () => {
    await ServicoRegistroIndividual.gerar({
      alunoCodigo: codigoEOL,
      dataInicio: dataInicioImpressaoRegistrosAnteriores,
      dataFim: dataFimImpressaoRegistrosAnteriores,
      turmaId: turmaSelecionada.id,
    })
      .then(() => {
        sucesso(
          'Solicitação de geração do relatório gerada com sucesso. Em breve você receberá uma notificação com o resultado.'
        );
      })
      .catch(e => erros(e));
  };

  useEffect(() => {
    const desabilitar =
      Object.keys(dadosRegistroAtual).length ||
      dadosPrincipaisRegistroIndividual?.registrosIndividuais?.items.length;

    dispatch(setMostrarMensagemSemHistorico(!desabilitar));
    setDesabilitarBotaoImprimir(!desabilitar);
  }, [
    dispatch,
    dadosPrincipaisRegistroIndividual,
    dadosRegistroAtual,
    desabilitarBotaoImprimir,
  ]);

  return (
    <DetalhesAluno
      exibirFrequencia={false}
      dados={dadosAlunoObjectCard}
      permiteAlterarImagem={!desabilitarCampos}
      onClickImprimir={gerar}
      desabilitarImprimir={
        !codigoEOL ||
        !dataInicioImpressaoRegistrosAnteriores ||
        !dataFimImpressaoRegistrosAnteriores ||
        desabilitarBotaoImprimir
      }
    />
  );
};

export default ObjectCardRegistroIndividual;

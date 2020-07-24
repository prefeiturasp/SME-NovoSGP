import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import Button from '~/componentes/button';
import { Colors } from '~/componentes/colors';
import { URL_HOME } from '~/constantes/url';
import {
  setRelatorioSemestralEmEdicao,
  setDadosRelatorioSemestral,
  limparDadosParaSalvarRelatorioSemestral,
} from '~/redux/modulos/relatorioSemestralPAP/actions';
import { confirmar } from '~/servicos/alertas';
import history from '~/servicos/history';
import servicoSalvarRelatorioSemestral from '../../servicoSalvarRelatorioSemestral';
import { ehTurmaInfantil } from '~/servicos/Validacoes/validacoesInfatil';

const BotoesAcoesRelatorioSemestral = () => {
  const dispatch = useDispatch();
  const usuario = useSelector(store => store.usuario);
  const { turmaSelecionada } = usuario;

  const modalidadesFiltroPrincipal = useSelector(
    store => store.filtro.modalidades
  );

  const alunosRelatorioSemestral = useSelector(
    store => store.relatorioSemestralPAP.alunosRelatorioSemestral
  );

  const relatorioSemestralEmEdicao = useSelector(
    store => store.relatorioSemestralPAP.relatorioSemestralEmEdicao
  );

  const dadosRelatorioSemestral = useSelector(
    store => store.relatorioSemestralPAP.dadosRelatorioSemestral
  );

  const desabilitarCampos = useSelector(
    store => store.relatorioSemestralPAP.desabilitarCampos
  );

  const onClickSalvar = () => {
    return servicoSalvarRelatorioSemestral.validarSalvarRelatorioSemestral(
      true
    );
  };

  const perguntaAoSalvar = async () => {
    return confirmar(
      'Atenção',
      '',
      'Suas alterações não foram salvas, deseja salvar agora?'
    );
  };

  const onClickVoltar = async () => {
    if (!desabilitarCampos && relatorioSemestralEmEdicao) {
      const confirmado = await perguntaAoSalvar();
      if (confirmado) {
        const salvou = await onClickSalvar();
        if (salvou) {
          history.push(URL_HOME);
        }
      } else {
        history.push(URL_HOME);
      }
    } else {
      history.push(URL_HOME);
    }
  };

  const recarregarDados = () => {
    dispatch(limparDadosParaSalvarRelatorioSemestral());
    dispatch(setRelatorioSemestralEmEdicao(false));

    const dados = dadosRelatorioSemestral;
    dispatch(setDadosRelatorioSemestral({ ...dados }));
  };

  const onClickCancelar = async () => {
    if (relatorioSemestralEmEdicao) {
      const confirmou = await confirmar(
        'Atenção',
        'Você não salvou as informações preenchidas.',
        'Deseja realmente cancelar as alterações?'
      );
      if (confirmou) {
        recarregarDados();
      }
    }
  };
  return (
    <>
      <Button
        id="btn-voltar-relatorio-semestral"
        label="Voltar"
        icon="arrow-left"
        color={Colors.Azul}
        border
        className="mr-2"
        onClick={onClickVoltar}
      />
      <Button
        id="btn-cancelar-relatorio-semestral"
        label="Cancelar"
        color={Colors.Roxo}
        border
        className="mr-2"
        onClick={onClickCancelar}
        disabled={
          desabilitarCampos ||
          !relatorioSemestralEmEdicao ||
          !alunosRelatorioSemestral ||
          alunosRelatorioSemestral.length < 1 ||
          !alunosRelatorioSemestral
        }
      />
      <Button
        id="btn-salvar-relatorio-semestral"
        label="Salvar"
        color={Colors.Roxo}
        border
        bold
        className="mr-2"
        onClick={onClickSalvar}
        disabled={
          ehTurmaInfantil(modalidadesFiltroPrincipal, turmaSelecionada)||
          desabilitarCampos ||
          !relatorioSemestralEmEdicao
        }
      />
    </>
  );
};

export default BotoesAcoesRelatorioSemestral;

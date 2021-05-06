import { Tooltip } from 'antd';
import * as moment from 'moment';
import PropTypes from 'prop-types';
import React, { useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import styled from 'styled-components';
import { DataTable } from '~/componentes';
import Button from '~/componentes/button';
import { Base, Colors } from '~/componentes/colors';
import Label from '~/componentes/label';
import { BtnExcluirDiasHorario } from '~/paginas/AEE/Plano/Cadastro/planoAEECadastro.css';
import { setResetarTabela } from '~/redux/modulos/questionarioDinamico/actions';
import { confirmar } from '~/servicos';
import { removerArrayAninhados } from '~/utils';
import QuestionarioDinamicoFuncoes from '../../Funcoes/QuestionarioDinamicoFuncoes';
import ModalCadastroDiasHorario from './modalCadastroDiasHorarios';

const DiasHorariosTabela = props => {
  const { label, questaoAtual, form, desabilitado, onChange } = props;

  const resetarTabela = useSelector(
    store => store.questionarioDinamico.resetarTabela
  );

  const dispatch = useDispatch();

  const [exibirModal, setExibirModal] = useState(false);
  const [dadosIniciais, setDadosIniciais] = useState({});

  const onClickNovoDiaHorario = () => {
    setExibirModal(true);
  };

  const onCloseModal = novosDados => {
    setExibirModal(false);
    setDadosIniciais();

    if (novosDados) {
      const dadosAtuais = form?.values?.[questaoAtual.id]?.length
        ? form?.values?.[questaoAtual.id]
        : [];
      if (novosDados?.id) {
        const indexItemAnterior = dadosAtuais.findIndex(
          x => x.id === novosDados.id
        );
        dadosAtuais[indexItemAnterior] = novosDados;
      } else {
        novosDados.id = dadosAtuais.length + 1;
        dadosAtuais.push(novosDados);
      }

      if (form) {
        form.setFieldValue(questaoAtual.id, dadosAtuais);
        onChange();
      }
    }
  };

  const onClickRow = row => {
    setDadosIniciais(row);
    setExibirModal(true);
  };

  const formatarCampoTabela = data => {
    let dataFormatada = '';
    if (data) {
      dataFormatada = moment(data).format('HH:mm');
    }
    return <span> {dataFormatada}</span>;
  };

  const acoes = {
    title: 'Ação',
    dataIndex: 'acaoRemover',
    render: (texto, linha) => {
      return (
        <Tooltip title="Excluir">
          <BtnExcluirDiasHorario>
            <Button
              id="btn-excluir"
              icon="trash-alt"
              iconType="far"
              color={Colors.Azul}
              border
              className="btn-excluir-dias-horario"
              disabled={desabilitado || questaoAtual.somenteLeitura}
              onClick={async e => {
                e.stopPropagation();
                if (!desabilitado) {
                  const confirmado = await confirmar(
                    'Excluir',
                    '',
                    'Você tem certeza que deseja excluir este registro?'
                  );

                  if (confirmado) {
                    const dadosAtuais = form?.values?.[questaoAtual.id]?.length
                      ? form?.values?.[questaoAtual.id]
                      : [];

                    const indice = dadosAtuais.findIndex(
                      item => item.id === linha.id
                    );
                    if (indice !== -1) {
                      dadosAtuais.splice(indice, 1);
                      form.setFieldValue(questaoAtual.id, dadosAtuais);
                      onChange();
                    }
                  }
                }
              }}
              height="30px"
              width="30px"
            />
          </BtnExcluirDiasHorario>
        </Tooltip>
      );
    },
  };

  const colunas = [
    {
      title: 'Dia da Semana',
      dataIndex: 'diaSemana',
    },
    {
      title: 'Horário de início',
      dataIndex: 'horarioInicio',
      render: data => formatarCampoTabela(data),
    },
    {
      title: 'Horário de término',
      dataIndex: 'horarioTermino',
      render: data => formatarCampoTabela(data),
    },
  ];

  if (!desabilitado) {
    colunas.push(acoes);
  }

  const Erro = styled.span`
    color: ${Base.Vermelho};
  `;

  const possuiErro = () => {
    return (
      form &&
      form.errors[String(questaoAtual.id)] &&
      form.touched[String(questaoAtual.id)]
    );
  };

  const obterErros = () => {
    return form &&
      form.touched[String(questaoAtual.id)] &&
      form.errors[String(questaoAtual.id)] ? (
      <Erro>{form.errors[String(questaoAtual.id)]}</Erro>
    ) : (
      ''
    );
  };

  useEffect(() => {
    if (resetarTabela) {
      const dadosRecuperados = questaoAtual?.resposta?.map(item =>
        JSON.parse(item.texto)
      );
      const dadosNaoAninhados = removerArrayAninhados(dadosRecuperados);
      form.setFieldValue(questaoAtual.id, dadosNaoAninhados);
      dispatch(setResetarTabela(false));
    }
  }, [dispatch, resetarTabela, questaoAtual, form]);

  return (
    <>
      <ModalCadastroDiasHorario
        onClose={onCloseModal}
        exibirModal={exibirModal}
        dadosIniciais={dadosIniciais}
      />
      <Label text={label} />
      <div className={possuiErro() ? 'tabela-invalida' : ''}>
        <DataTable
          columns={colunas}
          dataSource={
            form?.values?.[questaoAtual.id]?.length
              ? QuestionarioDinamicoFuncoes.ordenarDiasDaSemana(
                  form?.values?.[questaoAtual.id]
                )
              : []
          }
          pagination={false}
          onClickRow={onClickRow}
        />
      </div>
      {form ? obterErros() : ''}
      {!desabilitado ? (
        <Button
          id="btn-novo-dia-horario"
          label="Novo horário"
          icon="plus"
          color={Colors.Azul}
          border
          className="mr-3 mt-2"
          onClick={onClickNovoDiaHorario}
          disabled={desabilitado}
        />
      ) : (
        ''
      )}
    </>
  );
};

DiasHorariosTabela.propTypes = {
  questaoAtual: PropTypes.oneOfType([PropTypes.any]),
  form: PropTypes.oneOfType([PropTypes.any]),
  label: PropTypes.string,
  desabilitado: PropTypes.bool,
  onChange: PropTypes.func,
};

DiasHorariosTabela.defaultProps = {
  label: '',
  questaoAtual: null,
  form: null,
  desabilitado: false,
  onChange: () => {},
};

export default DiasHorariosTabela;

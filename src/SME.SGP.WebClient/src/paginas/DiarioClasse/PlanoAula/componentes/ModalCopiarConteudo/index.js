import React, { useState, useEffect } from 'react';
import t from 'prop-types';
import shortid from 'shortid';

// Redux
import { useSelector, useDispatch } from 'react-redux';
import { setLoaderModal } from '~/redux/modulos/loader/actions';

// Componentes
import {
  ModalConteudoHtml,
  Button,
  Colors,
  CampoData,
  Grid,
  Loader,
} from '~/componentes';
import TurmasDropDown from '~/componentes-sgp/TurmasDropDown';
import ListaCheckbox from './componentes/ListaCheckbox';

// Styles
import { Row } from './styles';

// Serviço
import api from '~/servicos/api';
import PlanoAulaServico from '~/servicos/Paginas/PlanoAula';
import AbrangenciaServico from '~/servicos/Abrangencia';
import { erros, erro, sucesso } from '~/servicos/alertas';

function ModalCopiarConteudo({
  show,
  disciplina,
  onClose,
  planoAula,
  dataAula,
}) {
  const filtro = useSelector(store => store.usuario.turmaSelecionada);
  const [carregando, setCarregando] = useState(false);
  const dispatch = useDispatch();
  const [confirmado, setConfirmado] = useState(false);
  const [alerta, setAlerta] = useState(false);
  const [listaTurmas, setListaTurmas] = useState([]);
  const [turmas, setTurmas] = useState([]);
  const [valoresCheckbox, setValoresCheckbox] = useState({
    objetivosAprendizagem: true,
    desenvolvimentoAula: true,
    recuperacaoContinua: false,
    licaoCasa: false,
  });

  useEffect(() => {
    async function buscaTurmas() {
      const { data } = await AbrangenciaServico.buscarTurmas(
        filtro.unidadeEscolar,
        filtro.modalidade
      );

      if (data) {
        setListaTurmas(
          data
            .filter(x => x.ano === filtro.ano)
            .map(item => ({
              desc: item.nome,
              valor: item.codigo,
            }))
        );
      }
    }
    buscaTurmas();
  }, [filtro.unidadeEscolar, filtro.modalidade, filtro.ano]);

  const adicionarTurma = () => {
    setTurmas([
      ...turmas,
      {
        id: shortid.generate(),
        turmaId: '',
        data: '',
        temErro: false,
        mensagemErro: 'Data já possui conteúdo',
      },
    ]);
  };

  const onClickExcluir = item => {
    setTurmas(turmas.filter(x => x.id !== item.id));
  };

  const { anoLetivo } = filtro;

  const onChangeTurma = async (turma, linha) => {
    if (turma)
      try {
        setCarregando(true);
        const { data, status } = await api.get(
          `v1/calendarios/frequencias/aulas/datas/${anoLetivo}/turmas/${turma}/disciplinas/${disciplina}`
        );
        setCarregando(false);
        if (data && status === 200) {
          setTurmas(
            turmas.map(x =>
              x.id === linha.id
                ? {
                    ...linha,
                    turmaId: turma,
                    diasParaHabilitar: data.map(y =>
                      window.moment(y.data).format('YYYY-MM-DD')
                    ),
                  }
                : x
            )
          );
        }
      } catch (error) {
        erro(error);
        setCarregando(false);
      }
  };

  const onChangeData = async (dataSelecionada, linha) => {
    setTurmas(
      turmas.map(x =>
        x.id === linha.id
          ? {
              ...linha,
              data: dataSelecionada,
            }
          : x
      )
    );
  };

  const onChangeCheckbox = (evento, campo) => {
    setValoresCheckbox({
      ...valoresCheckbox,
      [campo]: evento.target.checked,
    });
  };

  const onCloseModal = () => {
    setValoresCheckbox({
      objetivosAprendizagem: true,
      desenvolvimentoAula: true,
      recuperacaoContinua: false,
      licaoCasa: false,
    });
    setTurmas([]);
    setAlerta(false);
    onClose();
  };

  const copiar = async () => {
    dispatch(setLoaderModal(true));
    const {
      data: dados,
      status: resposta,
    } = await PlanoAulaServico.migrarPlano({
      idsPlanoTurmasDestino: turmas.map(x => ({
        ...x,
        sobreescrever: true,
      })),
      planoAulaId: planoAula.id,
      disciplinaId: disciplina,
      migrarLicaoCasa: valoresCheckbox.licaoCasa,
      migrarRecuperacaoAula: valoresCheckbox.recuperacaoContinua,
      migrarObjetivos: valoresCheckbox.objetivosAprendizagem,
    });
    if (dados || resposta === 200) {
      sucesso('Plano de aula copiado com sucesso!');
      dispatch(setLoaderModal(false));
      onCloseModal();
    }
  };

  const onClickSalvar = async () => {
    try {
      if (!confirmado) {
        setCarregando(true);
        const { data, status } = await PlanoAulaServico.verificarSeExiste({
          planoAulaTurmaDatas: turmas.map(x => ({
            data: x.data,
            turmaId: x.turmaId,
            disciplinaId: disciplina,
          })),
        });
        if (data && status === 200) {
          const temErro = data.filter(x => x.existe === true);
          if (temErro.length > 0) {
            temErro.forEach(err => {
              setTurmas(
                turmas.map(x =>
                  x.turmaId === String(err.turmaId)
                    ? {
                        ...x,
                        temErro: true,
                        mensagemErro: 'Data já possui conteúdo',
                      }
                    : x
                )
              );
            });
            setAlerta(true);
          } else await copiar();
          setConfirmado(true);
          setCarregando(false);
        }
      }

      if (confirmado) {
        await copiar();
      }
    } catch (error) {
      erros(error);
      dispatch(setLoaderModal(false));
    }
  };

  return (
    <ModalConteudoHtml
      titulo="Copiar conteúdo"
      visivel={show || alerta}
      closable
      onClose={() => onCloseModal()}
      onConfirmacaoSecundaria={() => onCloseModal()}
      onConfirmacaoPrincipal={() => onClickSalvar()}
      labelBotaoPrincipal="Confirmar"
      labelBotaoSecundario="Descartar"
      perguntaAtencao={
        alerta &&
        'Os planos de aula de algumas turmas, já possuem conteúdo que será sobrescrito. Deseja continuar?'
      }
      tituloAtencao={alerta && 'Atenção'}
      desabilitarBotaoPrincipal={turmas.length < 1}
    >
      <Loader loading={carregando}>
        {turmas.map(linha => (
          <Row key={shortid.generate()} className="row">
            <Grid cols={5}>
              <TurmasDropDown
                ueId={filtro.unidadeEscolar}
                modalidadeId={filtro.modalidade}
                valor={linha.turmaId}
                onChange={turma => onChangeTurma(turma, linha)}
                dados={listaTurmas}
                allowClear={false}
              />
            </Grid>
            <Grid cols={5}>
              <CampoData
                valor={linha.data}
                onChange={data => onChangeData(data, linha)}
                name="dataInicio"
                placeholder="DD/MM/AAAA"
                formatoData="DD/MM/YYYY"
                temErro={linha.temErro}
                mensagemErro={linha.mensagemErro}
                diasParaHabilitar={linha.diasParaHabilitar}
              />
            </Grid>
            <Grid cols={2}>
              <Button
                id={shortid.generate()}
                label="Excluir"
                color={Colors.Roxo}
                border
                className="btnGroupItem"
                onClick={() => onClickExcluir(linha)}
              />
            </Grid>
          </Row>
        ))}
        <div style={{ display: 'flex', justifyContent: 'flex-end' }}>
          <Button
            id={shortid.generate()}
            label="Adicionar turma"
            color={Colors.Roxo}
            border
            className="btnGroupItem"
            onClick={adicionarTurma}
          />
        </div>
        <div style={{ marginTop: '26px' }}>
          <ListaCheckbox
            valores={valoresCheckbox}
            onChange={(evento, campo) => onChangeCheckbox(evento, campo)}
          />
        </div>
      </Loader>
    </ModalConteudoHtml>
  );
}

ModalCopiarConteudo.propTypes = {
  show: t.bool,
  disciplina: t.string,
  onClose: t.func,
  planoAula: t.oneOfType([t.object]),
  dataAula: t.string,
};

ModalCopiarConteudo.defaultProps = {
  show: false,
  disciplina: null,
  onClose: null,
  planoAula: null,
  dataAula: null,
};

export default ModalCopiarConteudo;

import PropTypes from 'prop-types';
import React, { useLayoutEffect, useRef, useState } from 'react';
import { useSelector } from 'react-redux';
import styled from 'styled-components';
import { limparDadosFiltro } from '~/redux/modulos/filtro/actions';
import { setLoaderGeral } from '~/redux/modulos/loader/actions';
import {
  Deslogar,
  removerTurma,
  salvarDadosLogin,
} from '~/redux/modulos/usuario/actions';
import { erro } from '~/servicos/alertas';
import api from '~/servicos/api';
import { setMenusPermissoes } from '~/servicos/servico-navegacao';

import { Base } from '../componentes/colors';
import { store } from '../redux';
import { perfilSelecionado } from '../redux/modulos/perfil/actions';
import history from '../servicos/history';

const Perfil = props => {
  const { Botao, Icone, Texto } = props;
  const [ocultaPerfis, setarOcultaPerfis] = useState(true);
  const perfilStore = useSelector(e => e.perfil);
  const usuarioStore = useSelector(e => e.usuario);

  const listaRef = useRef();

  const ItensPerfil = styled.div`
    border-top-left-radius: 5px;
    border-bottom-left-radius: 5px;
    border-bottom-right-radius: 5px;
    width: 145px;
    height: auto;
    background: ${Base.Branco};
    border: solid ${Base.CinzaDesabilitado} 1px;
    position: absolute;
    right: 16%;
  `;

  const Item = styled.tr`
    text-align: left;
    width: 100%;
    height: 100%;
    vertical-align: middle !important;

    &:not(:last-child) {
      border-bottom: solid ${Base.CinzaDesabilitado} 1px !important;
    }

    &:hover {
      cursor: pointer;
      background: #e7e6f8;
      font-weight: bold !important;
    }

    td {
      height: 35px;
      font-size: 10px;
      padding-left: 7px;
      width: 145px;
    }

    i {
      font-size: 14px;
      color: #707683;
    }
  `;

  const ContainerIcone = styled.div`
    background: ${perfilStore.perfis.length > 1
      ? Base.Roxo
      : Base.CinzaDesabilitado};
    color: ${Base.Branco};
    font-size: 18px !important;
    height: 28px !important;
    width: 28px !important;
    vertical-align: middle;
    box-sizing: border-box;
    align-items: center !important;
    border-radius: 50%;
    display: inline-block;
    justify-content: center !important;
    i {
      background: ${perfilStore.perfis.length > 1
        ? Base.Roxo
        : Base.CinzaDesabilitado} !important;
    }
  `;

  const limparFiltro = () => {
    store.dispatch(limparDadosFiltro());
    store.dispatch(removerTurma());
  };

  const cancelarRequisicoesPendentes = () => {
    api.CancelarRequisicoes('Cancelado pelo usuário');
  };

  const gravarPerfilSelecionado = perfil => {
    if (perfil) {
      const perfilNovo = perfilStore.perfis.filter(
        item => item.codigoPerfil === perfil
      );

      if (
        perfilStore.perfilSelecionado.codigoPerfil !==
        perfilNovo[0].codigoPerfil
      ) {
        store.dispatch(setLoaderGeral(true));
        api
          .put(`v1/autenticacao/perfis/${perfilNovo[0].codigoPerfil}`)
          .then(resp => {
            const {
              token,
              ehProfessor,
              ehProfessorCj,
              ehProfessorPoa,
              dataHoraExpiracao,
              ehProfessorInfantil,
              ehProfessorCjInfantil,
            } = resp.data;

            const {
              rf,
              modificarSenha,
              possuiPerfilSmeOuDre,
              possuiPerfilDre,
              possuiPerfilSme,
              menu,
            } = usuarioStore;

            store.dispatch(
              salvarDadosLogin({
                token,
                rf,
                usuario: usuarioStore,
                modificarSenha,
                possuiPerfilSmeOuDre,
                possuiPerfilDre,
                possuiPerfilSme,
                ehProfessorCj,
                ehProfessor,
                menu,
                ehProfessorPoa,
                dataHoraExpiracao,
                ehProfessorInfantil,
                ehProfessorCjInfantil,
              })
            );

            setMenusPermissoes();
            limparFiltro();
            store.dispatch(perfilSelecionado(perfilNovo[0]));
            setTimeout(() => {
              store.dispatch(setLoaderGeral(false));
            }, 1000);
          })
          .catch(() => {
            erro('Sua sessão expirou');
            setTimeout(() => {
              store.dispatch(Deslogar());
            }, 2000);
          });
        history.push('/');
      } else {
        store.dispatch(perfilSelecionado(perfilNovo[0]));
        limparFiltro();
      }
    } else {
      limparFiltro();
    }
  };

  const onClickPerfil = e => {
    cancelarRequisicoesPendentes();
    gravarPerfilSelecionado(e.currentTarget.accessKey);
  };

  const onClickBotao = () => {
    if (perfilStore.perfis.length > 1) {
      setarOcultaPerfis(!ocultaPerfis);
    }
  };

  const handleClickFora = event => {
    if (listaRef.current && !listaRef.current.contains(event.target)) {
      setarOcultaPerfis(true);
    }
  };

  useLayoutEffect(() => {
    if (ocultaPerfis) document.addEventListener('click', handleClickFora);
    else document.removeEventListener('click', handleClickFora);
  }, [ocultaPerfis]);

  return (
    <div className="position-relative" ref={listaRef}>
      <Botao
        className="text-center stretched-link"
        onClick={onClickBotao}
        disabled={perfilStore.perfis.length <= 1}
      >
        <ContainerIcone>
          <Icone className="fas fa-user-circle" />
        </ContainerIcone>
        <Texto
          className={`d-block mt-1 ${ocultaPerfis ? '' : ' font-weight-bold'}`}
        >
          {perfilStore.perfilSelecionado.sigla
            ? perfilStore.perfilSelecionado.sigla
            : perfilStore.perfilSelecionado.nomePerfil}
        </Texto>
      </Botao>
      {ocultaPerfis}
      <ItensPerfil hidden={ocultaPerfis} className="list-inline">
        <table>
          <tbody>
            {perfilStore.perfis.map(item => (
              <Item
                key={item.codigoPerfil}
                onClick={onClickPerfil}
                accessKey={item.codigoPerfil}
              >
                <td style={{ width: '20px' }}>
                  <i
                    value={item.codigoPerfil}
                    className="fas fa-user-circle"
                  ></i>
                </td>
                <td
                  style={{
                    width: '100%',
                    fontWeight:
                      item.codigoPerfil ===
                      perfilStore.perfilSelecionado.codigoPerfil
                        ? 'bold'
                        : 'initial',
                  }}
                >
                  {item.nomePerfil + (item.sigla ? '(' + item.sigla + ')' : '')}
                </td>
              </Item>
            ))}
          </tbody>
        </table>
      </ItensPerfil>
    </div>
  );
};

Perfil.propTypes = {
  Botao: PropTypes.object.isRequired,
  Icone: PropTypes.object.isRequired,
  Texto: PropTypes.object.isRequired,
};

export default Perfil;

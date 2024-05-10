FROM node:16-alpine as build-deps
WORKDIR /usr/src/app
# COPY src/SME.SGP.WebClient/package.json src/SME.SGP.WebClient/yarn.lock ./

ENV NODE_OPTIONS --max_old_space_size=4096

COPY /src/SME.SGP.WebClient/ .
RUN set NODE_OPTIONS=--max_old_space_size=4096 && \ 
    yarn install && \
    yarn build

FROM nginx:1.17-alpine

COPY src/SME.SGP.WebClient/configuracoes/default.conf /etc/nginx/conf.d/
COPY --from=build-deps /usr/src/app/build /usr/share/nginx/html
EXPOSE 80

## startup.sh script is launched at container run
ADD src/SME.SGP.WebClient/docker/startup.sh /startup.sh
RUN dos2unix "/startup.sh"
RUN ["chmod", "+x", "/startup.sh"]
CMD /startup.sh


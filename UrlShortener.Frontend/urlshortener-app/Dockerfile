FROM node:18
WORKDIR /usr/src/app
COPY . .
RUN npm install && npm run build

FROM nginx:latest
COPY --from=0 /usr/src/app/docker/default.conf /etc/nginx/conf.d/default.conf
COPY --from=0 /usr/src/app/docker/run.sh /config/run.sh
COPY --from=0 /usr/src/app/build /usr/share/nginx/html
RUN chmod +x /config/run.sh
EXPOSE 80
ENTRYPOINT ["/bin/sh", "/config/run.sh"]
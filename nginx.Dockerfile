# задача контейнера запустить Nginx с сертификатами, чтобы он пробрасывал внутрь запросы к конкретным серверам фронта и бэка
FROM nginx:latest
COPY nginx.conf /etc/nginx/nginx.conf
COPY certificate/birdegop.crt /etc/nginx/birdegop.crt
COPY certificate/birdegop.key /etc/nginx/birdegop.key
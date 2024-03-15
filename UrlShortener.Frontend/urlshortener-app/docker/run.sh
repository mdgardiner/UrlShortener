echo 'Using configuration template:'
cat /usr/share/nginx/html/config.template.js
echo ''

cp /usr/share/nginx/html/config.template.js /usr/share/nginx/html/config.js

echo 'Configuration before updates:'
cat /usr/share/nginx/html/config.js
echo ''

echo "Replacing REACT_APP_SHORTENER_API_BASE_URL to '$REACT_APP_SHORTENER_API_BASE_URL'"
sed -i "s|{{REACT_APP_SHORTENER_API_BASE_URL}}|$REACT_APP_SHORTENER_API_BASE_URL|g" /usr/share/nginx/html/config.js

echo 'Using following configuration:'
cat /usr/share/nginx/html/config.js
echo ''

nginx -g 'daemon off;'
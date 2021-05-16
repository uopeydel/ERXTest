FROM nginx:alpine 
 
WORKDIR /usr/share/nginx/html  
COPY * ./ 
COPY nginx.conf /etc/nginx/nginx.conf   
COPY start.sh ./  
RUN sed -i -e "s/\r$//" start.sh  
# The command that will be executed when the container starts  
CMD /bin/sh ./start.sh 
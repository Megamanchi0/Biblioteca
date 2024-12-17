Proyecto de gestión de una biblioteca y de manejo de archivos realizado con fines de aprendizaje. Se implementó una API RESTful, consumo de APIs externas, autenticación con JWT, entre otras cosas.

Para ejecutar el proyecto:

1. Asegurarse de tener instalado Angular y .NET.
2. Restaurar el back-up de la base de datos en SQL Server.
3. Crear una variable de entorno con el nombre "DB_BIBLIOTECA" que contenga la cadena de conexión a la base de datos.
  Ejemplo de cadena de conexión: Data Source=[Nombre_dispositivo];Database=DBBiblioteca;Trusted_Connection=True;MultipleActiveResultSets=True;user id=[usuario];pwd=[contraseña];TrustServerCertificate=true;
4. Si se desea usar la API (Send Grid) para enviar el correo de confirmación al registrarse, debe escribir al correo menlockfull@gmail.com solicitando la API Key y luego crear una variable de entorno con el nombre "API_KEY" y su valor correspondiente, ya que no es posible ponerla directamente en este repositorio.
5. En el proyecto biblioteca-frontend ejecutar el comando "npm install" para instalar las dependencias faltantes de Node.
6. Ejecutar los servidores del back-end y front-end, respectivamente.

Nota: El perfil con el rol administrador tiene acceso a todas las funcionalidades y es el siguiente:
- Correo: sebas@gmail.com
- Contraseña: sebas1234

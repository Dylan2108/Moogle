Para la ejecución de este proyecto se debe abrir una terminal en la carpeta Moogle-Dylan
y ejecutar el comando make dev.Al ejecutar este comando saldrá en consola Cargando Documentos,
esto es debido a que se esta ejecutando el metodo CargarDocumentos de la clase ProcesarDocumentos.
Este se encarga de todo el preprocesamiento de datos de los textos.Al cargarse estos datos saldrá
en consola Documentos Cargados y ya Moogle estara listo para realizar su búsqueda(este proceso de
cargar los documentos no debe tardar mas de un minuto).Ya a la hora de realizar la búsqueda 
escribimos una query y hacemos click en el boton Buscar(En la query no se deben poner operadores 
ya que estos no fueron implementados).Ya realizada la búsqueda si la query fue encontrada con 
éxito nos saldrán los documentos que más se asemejan a esta con un fragmento de texto.En caso de
que no se encuentre dicha query se da una sugerencia de que palabra quiso decir el usuario.
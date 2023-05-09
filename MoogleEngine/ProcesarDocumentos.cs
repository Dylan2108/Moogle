namespace MoogleEngine;
public class ProcesarDocumentos
{
 internal static List<List<string>>documentos = new();//Crea la lista de listas
 internal static Dictionary<string,double>IDF = new();//Crea el diccionario de IDF
 internal static List<Dictionary<string,double>>Lista_TF = new();//Crea la lista de diccionarios de TF
 internal static List<Dictionary<string,double>>Lista_tfidf = new();//Crea la lista de diccionarios de TFIDF
 internal static double[]? magnitud_documentos;//Crea el array para la magnitud de los documentos
 public static void CargarDocumentos()
 {//Realiza el preprocesamiento
  documentos = ProcesarDocumentos.LeerTextos();//Almacena la lista de listas
  ProcesarDocumentos.CalcularTFIDF(IDF,Lista_TF,documentos);
  Lista_tfidf = ProcesarDocumentos.TFIDF(IDF,Lista_TF);//Almacena tfidf en una lista de diccionarios
  magnitud_documentos = ProcesarDocumentos.Magnitud_Documentos(Lista_tfidf);//Calcula magnitud de los documentos
 } 
 public static List<List<string>>LeerTextos()//Devuelve una lista de los documentos que contiene y a su vez una lista de las palabras
 {
    List<List<string>>documentos = new();
    string[]nombredocumentos = ObtenerDocumentos();
    char[] separadores = {' ','=','`',';','\'','\t','.',',',':','-','_','/','+','%','?','[',']','(',')','{','}','|','<','>','!','#','&','@'};
    for(int i = 0;i<nombredocumentos.Length;i++)
    {
      documentos.Add(Tokenizar(File.ReadAllText(nombredocumentos[i]),separadores));
    }                               
    return documentos;                     
 }                       
 public static string[] ObtenerDocumentos()//Devuelve la ruta de todos los documentos 
 {
    string ruta = Directory.GetCurrentDirectory();
    ruta = Path.Join(ruta,"..","/Content");
    return Directory.GetFiles(ruta,"*txt",SearchOption.AllDirectories);
 }
 public static List<string> Tokenizar(string texto,char[]separadores)//Devuelve una lista de las palabras separadas
 {
  texto = ModificarTexto(texto);
  return texto.Split(separadores,StringSplitOptions.RemoveEmptyEntries).ToList();
 }
 public static string ModificarTexto(string texto)//Elimina caracteres y lleva a minusculas
 {
    texto = texto.Replace('\n',' ').Replace('\r',' ').ToLower();
    return texto;
 }
 public static void CalcularTFIDF(Dictionary<string,double>IDF,List<Dictionary<string,double>>Lista_TF,List<List<string>>documentos)
 {
   for(int i = 0;i<documentos.Count;i++)
   {
      Dictionary<string,double>TF = new();
      for(int j = 0;j<documentos[i].Count;j++)
      {
         if(!TF.ContainsKey(documentos[i][j]))
         {
            TF.Add(documentos[i][j],1);
         }
         else
         {
            TF[documentos[i][j]]++;
         }
         if(!IDF.ContainsKey(documentos[i][j]))
         {
            IDF.Add(documentos[i][j],1);
         }
         else if(TF[documentos[i][j]]==1)
         {
            IDF[documentos[i][j]]++;
         }
      }
      Lista_TF.Add(TF); 
   }
   for(int i = 0;i<Lista_TF.Count;i++)
   {
      foreach(KeyValuePair<string,double>elemento in Lista_TF[i])
      {
         Lista_TF[i][elemento.Key]/=Lista_TF[i].Count;
      }
   }
   foreach(KeyValuePair<string,double>elemento in IDF)
   {
      if(IDF[elemento.Key]!=0)
      {
         IDF[elemento.Key] = Math.Log2(documentos.Count/IDF[elemento.Key]);
      }
      else
      {
         IDF[elemento.Key] = 0;
      }
   }
 }
 public static List<Dictionary<string,double>>TFIDF(Dictionary<string,double>idf,List<Dictionary<string,double>>Lista_tf)
 {
  List<Dictionary<string,double>>TFIDF = new();
  foreach(Dictionary<string,double>tfdic in Lista_tf)
  {
   foreach(KeyValuePair<string,double>elemento in tfdic)
   {
      tfdic[elemento.Key] *= idf[elemento.Key];
   }
   TFIDF.Add(tfdic);
  }
  return TFIDF;
 }
 public static double[]Magnitud_Documentos(List<Dictionary<string,double>>tfidfs_doc)
 {
   double[]magnitud_documentos = new double[tfidfs_doc.Count];
   for(int i = 0;i<magnitud_documentos.Length;i++)
   {
      double suma = 0;
      foreach(KeyValuePair<string,double>elemento in tfidfs_doc[i])
      {
       suma += elemento.Value*elemento.Value;
      }
      magnitud_documentos[i] = Math.Sqrt(suma);
   }
   return magnitud_documentos;
 }
}
namespace MoogleEngine;


public static class Moogle
{
    public static SearchResult Query(string query) 
    {
        char[] separadores_query = {' ','=','`',';','\'','\t','.',',',':','-','_','/','+','%','?','[',']','(',')','{','}','|','<','>','!','#','&','@'};
        List<string>query_lista = ProcesarDocumentos.Tokenizar(query,separadores_query);
        Dictionary<string,double>query_tfidf = Consulta.TFIDF_Query(ProcesarDocumentos.IDF,query_lista);
        double query_magnitud = Consulta.Magnitud_Query(query_tfidf);
        double[] similitudcosenos = Consulta.SimilitudCosenos(query_tfidf,ProcesarDocumentos.Lista_tfidf,query_magnitud,ProcesarDocumentos.magnitud_documentos);
        int[] posicion_doc = Busqueda.PosiciondelosDocumentosImportantes(similitudcosenos);
        if(posicion_doc[0]!=-1)
        {
            string[]documentos_ruta = Busqueda.Documentosmasimportantes(similitudcosenos,posicion_doc);
            string[] snippets = new string[documentos_ruta.Length];
            SearchItem[] items = new SearchItem[snippets.Length];
            for(int i = 0;i<items.Length;i++)
            {
                snippets[i] = Busqueda.ObtenerSnippet(documentos_ruta[i],query_tfidf,ProcesarDocumentos.documentos[posicion_doc[i]]);
                items[i] = new SearchItem(Path.GetFileNameWithoutExtension(documentos_ruta[i]),snippets[i],similitudcosenos[i],"file://"+documentos_ruta[i]);
            }
            return new SearchResult(items);
        }
        else
        {
            SearchItem[] items = new SearchItem[1];
            items[0] = new SearchItem("No se encontraron resultados","Pruebe a hacer otra busqueda",0,"#");
            return new SearchResult(items,Consulta.Sugerencia(ProcesarDocumentos.IDF,query_lista));
        }
    }
}


namespace MoogleEngine;
internal static class Busqueda
{
    public static int[]PosiciondelosDocumentosImportantes(double[]similitudcoseno)
    {
        List<int>posiciones = new();
        for(int i = 0;i<5;i++)
        {
            double temporal = 0;
            int posicion = -1;
            for(int j = 0;j<similitudcoseno.Length;j++)
            {
                if(similitudcoseno[j]>temporal)
                {
                    temporal = similitudcoseno[j];
                    posicion = j;
                }
            }
            if(posicion!=-1)
            {
                posiciones.Add(posicion);
                similitudcoseno[posicion] = -1;
            }
            else
            {
                break;
            }
        }
        if(posiciones.Count!=0)
        {
            return posiciones.ToArray();
        }
        else
        {
            posiciones.Add(-1);
            return posiciones.ToArray();
        }
    }
    public static string[] Documentosmasimportantes(double[]cosenos,int[]posiciondedocumentos)
    {
        string[]documentosmasimportantes = new string[posiciondedocumentos.Length];
        for(int i = 0;i<documentosmasimportantes.Length;i++)
        {
         documentosmasimportantes[i] = ProcesarDocumentos.ObtenerDocumentos()[posiciondedocumentos[i]];
        }
        return documentosmasimportantes;
    }
    public static List<string>OrdenarQuery(Dictionary<string,double>query_tfidf)
    {
        var Dict_ordenado = from entry in query_tfidf orderby entry.Value descending select entry;
        List<string>lista_ordenada = new();
        foreach(KeyValuePair<string,double>entry in Dict_ordenado)
        {
            lista_ordenada.Add(entry.Key);
        }
        return lista_ordenada;
    }
    public static string ObtenerSnippet(string ruta,Dictionary<string,double>query_tfidf,List<string>documento)
    {
        List<string>query_lista = OrdenarQuery(query_tfidf);
        string doc_leido = File.ReadAllText(ruta).Replace('\n',' ').Replace('\r',' ');
        char[] separadores = {' ','\n','\t'};
        string doc_leido_modificado = ProcesarDocumentos.ModificarTexto(doc_leido);
        string snippet = "";
        for(int i = 0;i<query_lista.Count;i++)
        {
            if(documento.Contains(query_lista[i]))
            {
                for(int j = 0;j<documento.Count;j++)
                {
                    if(documento[j]==query_lista[i])
                    {
                        break;
                    }
                }
                int posicion = doc_leido_modificado.IndexOf(" "+query_lista[i]+" ");
                int ultima_posicion = 0;
                while(posicion==-1)
                {
                    if(!Char.IsLetter(doc_leido_modificado,doc_leido_modificado.IndexOf(query_lista[i],ultima_posicion)+query_lista[i].Length)&&!Char.IsLetter(doc_leido_modificado,doc_leido_modificado.IndexOf(query_lista[i],ultima_posicion)-1))
                    {
                        posicion = doc_leido_modificado.IndexOf(query_lista[i],ultima_posicion);
                    }
                    else
                    {
                        ultima_posicion = doc_leido_modificado.IndexOf(query_lista[i],ultima_posicion+query_lista[i].Length);
                    }
                }
                if(doc_leido_modificado.Length>posicion+200)
                {
                    snippet = "..." + doc_leido.Substring(posicion,200);
                    for(int k =posicion+200;k<doc_leido_modificado.Length&&Char.IsLetter(doc_leido[k]);k++)
                    {
                        snippet += doc_leido[k];
                    }
                    snippet += "...";
                }
                else
                {
                    snippet = "..." + doc_leido.Substring(posicion,doc_leido.Length-posicion);
                }
                break;
            }
        }
        return snippet;
    }
}
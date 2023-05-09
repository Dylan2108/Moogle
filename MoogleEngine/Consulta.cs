namespace MoogleEngine;
internal static class Consulta
{
    public static Dictionary<string,double>TFIDF_Query(Dictionary<string,double>idf,List<string>query)
    {
        Dictionary<string,double>tfidf_query = new();
        foreach(string palabra in query)
        {
            if(!tfidf_query.ContainsKey(palabra))
            {
                tfidf_query.Add(palabra,1);
            }
            else
            {
                tfidf_query[palabra]++;
            }
        }
        foreach(KeyValuePair<string,double>palabra in tfidf_query)
        {
            if(idf.ContainsKey(palabra.Key))
            {
                tfidf_query[palabra.Key] *= idf[palabra.Key];
            }
            else
            {
                tfidf_query[palabra.Key] = 0;
            }
        }
        return tfidf_query;
    }
    public static double Magnitud_Query(Dictionary<string,double>tfidf_query)
    {
        double suma_query = 0;
        foreach(KeyValuePair<string,double>elemento in tfidf_query)
        {
            suma_query += elemento.Value*elemento.Value;
        }
        return Math.Sqrt(suma_query);
    }
    public static double[] SimilitudCosenos(Dictionary<string,double>query_tfidf,List<Dictionary<string,double>>lista_tfidf,double magnitud_query,double[]magnitud_documentos)
    {
        double[] suma = new double[lista_tfidf.Count];
        for(int i = 0;i<lista_tfidf.Count;i++)
        {
            foreach(KeyValuePair<string,double>elemento in query_tfidf)
            {
                if(lista_tfidf[i].ContainsKey(elemento.Key))
                {
                    suma[i] += elemento.Value*lista_tfidf[i][elemento.Key]; 
                }
            }
        }
        double[] similitudcosenos = new double[lista_tfidf.Count];
        for(int i = 0;i<similitudcosenos.Length;i++)
        {
            double magnitudes = magnitud_query*magnitud_documentos[i];
            if(magnitudes!=0)
            {
                similitudcosenos[i] = (suma[i]/magnitudes);
            }
            else
            {
                similitudcosenos[i] = 0;
            }
        }
        return similitudcosenos;
    }
    public static string Sugerencia(Dictionary<string,double>idf,List<string>lista_query)
    {
        string[] palabra_similar = new string[lista_query.Count];
        int distancia;
        for(int i = 0;i<lista_query.Count;i++)
        {
            int comparador = int.MaxValue;
            if(!idf.ContainsKey(lista_query[i]))
            {
                foreach(KeyValuePair<string,double>palabra in idf)
                {
                    distancia = Similar(palabra.Key,lista_query[i]);
                    if(distancia<comparador)
                    {
                     comparador = distancia;
                     palabra_similar[i] = palabra.Key;
                    } 
                }
            }
            else
            {
                palabra_similar[i] = lista_query[i];
            }
        }
        return String.Join(' ',palabra_similar);
        static int Similar(string a , string b)
        {
            int distancia = 0;
            if(a.Length>b.Length)
            {
                string c = b;
                b = a;
                a = c;
            }
            for(int i = 0;i<a.Length;i++)
            {
                if(a[i]!=b[i])
                {
                    distancia++;
                }
            }
            distancia += Math.Abs(b.Length-a.Length);
            return distancia;
        }
    }
}
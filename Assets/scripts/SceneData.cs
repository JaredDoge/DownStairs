using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneData 
{
    // Start is called before the first frame update
    static SceneData instance;

    Dictionary<string,object> data = new Dictionary<string, object>(); 

    public static void Set(string key,object newData)
    {
        if(instance==null){
            instance=new SceneData();
        }
        instance.data[key]=newData;
    }

    public static bool HasKey(string key)
    {
        return instance!=null && instance.data.ContainsKey(key);
    }

    public static T Get<T>(string key) //where T : class
    {
        if(HasKey(key))
        {
            
            Debug.Log(instance.data.Count +" 勝");
            return (T)(instance.data[key]);
        }
        return default(T);
    }

    public static T Pop<T>(string key) 
    {
        if(HasKey(key))
        {
            T data=(T)(instance.data[key]); 
            instance.data.Remove(key);   
            return data;
        } 
        return default(T);
    }
    
    public static void Clear()
    {

        if(instance!=null)
        {
            instance.data.Clear();
            instance=null;
        }        
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.InteropServices.ComTypes;

public class FileDataHandler 
{
    private string dataDirPath = "";

    private string dataFileName = "";
    private bool useEncryption = false;
    private readonly string encryptionCodeWord = "word";

    public FileDataHandler(string dataDirPath, string dataFileName, bool useEncryption)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
        this.useEncryption = useEncryption;
    }

    public GameData Load()
    {
        //u¿yj path.combine do obs³ugi plików w innych systemach operacyjnych posiadaj¹cych inne separatory
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        GameData loadedData = null;
        if(File.Exists(fullPath))
        {
            try
            {
                //za³aduj zserializowane dane z pliku
                string dataToLoad = "";
                using(FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                //opcjonalnie zaszyfruj dane
                if (useEncryption)
                {
                    dataToLoad = EncryptDecrypt(dataToLoad);
                }

                //zdeserializuj dane z pliku json z powrotem do obiektu c#
                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch(Exception e)
            {
                Debug.LogError("Error occured when trying to load data from file: " + fullPath + "\n" + e);
            }
        }
        return loadedData;
    }
    public void Save(GameData data)
    {   //u¿yj path.combine do obs³ugi plików w innych systemach operacyjnych posiadaj¹cych inne separatory
        string fullPath = Path.Combine(dataDirPath, dataFileName);
       try
        {
            //utwórz scie¿kê dla pliku jeœli ta nie istnieje
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            // serializuj dane gry do pliku Json
            string dataToStore = JsonUtility.ToJson(data, true);
            //opcjonalnie zaszyfruj dane
            if(useEncryption)
            {
                dataToStore = EncryptDecrypt(dataToStore);
            }

            //zapisz zserializowane dane do pliku
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch(Exception e) 
        {
            Debug.LogError("Error occured when trying to save data to file: " + fullPath + "\n" + e);
        }
    }
     //implementacja szyfrowania XOR
    private string EncryptDecrypt(string data)
    {
        string modifiedData = "";
        for(int i = 0; i < data.Length; i++)
        {
            modifiedData += (char)(data[i] ^ encryptionCodeWord[i % encryptionCodeWord.Length]);
        }
        return modifiedData;
    }
}

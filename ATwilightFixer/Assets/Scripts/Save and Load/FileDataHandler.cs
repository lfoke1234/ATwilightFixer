using System;
using System.IO;
using UnityEngine;

public class FileDataHandler
{
    private string dataDirPath = ""; // ������ ���� ���
    private string dataFileName = ""; // ������ ���� �̸�

    public FileDataHandler(string _dataDirPath, string _dataFileName)
    {
        dataDirPath = _dataDirPath;
        dataFileName = _dataFileName;
    }

    // ����� ������ ����
    public void Delete()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        if (File.Exists(fullPath))
            File.Delete(fullPath);
    }

    public void Save(GameData _data)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName); // ������ ������ ��ü ��θ� ����

        try
        {
            // ������ ���丮�� �������� �ʴ� ���, ���丮�� ����
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            // GameData ��ü�� JSON ������ ���ڿ��� ��ȯ
            string dataToStore = JsonUtility.ToJson(_data, true);

            // ���� ��Ʈ���� ����Ͽ� ������ �����ϰų� ���� ������ ������
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                // ��Ʈ���� ����� StreamWriter�� ����Ͽ� �����͸� �ۼ�
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore); // JSON ������ �ۼ�
                }
            }
        }
        // ���� �� ���ܰ� �߻��ϸ� ���� �޽����� ���
        catch (System.Exception e)
        {
            Debug.Log("Error on trying to save data to file: " + fullPath + "\n" + e);
            throw; // ���ܸ� �ٽ� �߻����� ȣ���� �������� ���ܸ� ó��
        }
    }


    public GameData Load()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName); // �ε��� ������ ��ü ��θ� ����.
        GameData loadData = null;

        // ������ �����ϴ��� Ȯ��
        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";

                // ���� ��Ʈ���� ����Ͽ� ���Ͽ���
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    // ��Ʈ���� ����� StreamReader�� ����Ͽ� �����͸� �б�
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd(); // ��� �����͸� �б�
                    }
                }

                // JSON ������ ���ڿ��� GameData ��ü�� ������ȭ
                loadData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            // �ε� �� ���ܰ� �߻��ϸ� ���� �޽����� ���
            catch (Exception e)
            {
                Debug.Log("Error on trying to load data from file: " + fullPath + "\n" + e);
            }
        }

        return loadData; // �����͸� �ε����� ���� ��� null�� ��ȯ
    }


}

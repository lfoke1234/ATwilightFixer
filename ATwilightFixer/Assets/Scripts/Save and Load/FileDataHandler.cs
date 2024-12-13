using System;
using System.IO;
using UnityEngine;

public class FileDataHandler
{
    private string dataDirPath = ""; // 데이터 파일 경로
    private string dataFileName = ""; // 데이터 파일 이름

    public FileDataHandler(string _dataDirPath, string _dataFileName)
    {
        dataDirPath = _dataDirPath;
        dataFileName = _dataFileName;
    }

    // 저장된 데이터 삭제
    public void Delete()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        if (File.Exists(fullPath))
            File.Delete(fullPath);
    }

    public void Save(GameData _data)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName); // 저장할 파일의 전체 경로를 설정

        try
        {
            // 데이터 디렉토리가 존재하지 않는 경우, 디렉토리를 생성
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            // GameData 객체를 JSON 형식의 문자열로 변환
            string dataToStore = JsonUtility.ToJson(_data, true);

            // 파일 스트림을 사용하여 파일을 생성하거나 기존 파일을 덮어씌우기
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                // 스트림에 연결된 StreamWriter를 사용하여 데이터를 작성
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore); // JSON 데이터 작성
                }
            }
        }
        // 저장 중 예외가 발생하면 오류 메시지를 출력
        catch (System.Exception e)
        {
            Debug.Log("Error on trying to save data to file: " + fullPath + "\n" + e);
            throw; // 예외를 다시 발생시켜 호출한 곳에서도 예외를 처리
        }
    }


    public GameData Load()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName); // 로드할 파일의 전체 경로를 설정.
        GameData loadData = null;

        // 파일이 존재하는지 확인
        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";

                // 파일 스트림을 사용하여 파일열기
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    // 스트림에 연결된 StreamReader를 사용하여 데이터를 읽기
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd(); // 모든 데이터를 읽기
                    }
                }

                // JSON 형식의 문자열을 GameData 객체로 역직렬화
                loadData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            // 로드 중 예외가 발생하면 오류 메시지를 출력
            catch (Exception e)
            {
                Debug.Log("Error on trying to load data from file: " + fullPath + "\n" + e);
            }
        }

        return loadData; // 데이터를 로드하지 못한 경우 null을 반환
    }


}

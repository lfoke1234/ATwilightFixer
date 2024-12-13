// ISaveManager 인터페이스는 저장 및 로드 기능을 구현하기 위한 인터페이스
// 이 인터페이스를 상속받는 클래스는 LoadData와 SaveData 메서드를 구현해야 된다.
public interface ISaveManager
{
    void LoadData(GameData _data);
    void SaveData(ref GameData _data);
}

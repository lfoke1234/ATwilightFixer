// ISaveManager �������̽��� ���� �� �ε� ����� �����ϱ� ���� �������̽�
// �� �������̽��� ��ӹ޴� Ŭ������ LoadData�� SaveData �޼��带 �����ؾ� �ȴ�.
public interface ISaveManager
{
    void LoadData(GameData _data);
    void SaveData(ref GameData _data);
}

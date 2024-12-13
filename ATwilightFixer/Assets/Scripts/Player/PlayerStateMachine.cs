using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine
{
    // ���� ����
    public PlayerState currentState { get; private set; }

    // ���۽� �ʱ�ȭ �޼���
    public void Initialize(PlayerState _startState)
    {
        currentState = _startState;
        currentState.Enter();
    }

    // ���� ���·� ��ȯ�ϴ� �ż���
    public void ChangeState(PlayerState newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }
}
